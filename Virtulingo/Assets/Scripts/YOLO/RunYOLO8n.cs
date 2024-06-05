using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Lays = Unity.Sentis.Layers;
using System.IO;
using SimpleFileBrowser;
using FF = Unity.Sentis.Functional;

/*
 *  YOLOv8n Inference Script
 *  ========================
 * 
 * Place this script on the Main Camera.
 * 
 * Place the yolob8n.sentis file in the asset folder and drag onto the asset field
 * Place a *.mp4 video file in the Assets/StreamingAssets folder
 * Create a RawImage in your scene and set it as the displayImage field
 * Drag the classes.txt into the labelsAsset field
 * Add a reference to a sprite image for the bounding box and a font for the text
 * 
 */


public class RunYOLO8n : MonoBehaviour
{
    // Drag the yolov8n.sentis file here
    public ModelAsset asset;
    // Drag the classes.txt file here
    public TextAsset labelsAsset;
    // Drag the translation file here
    public TextAsset translationJsonFile;
    // Create a Raw Image in the scene and link it here:
    public RawImage displayImage;
    // Link to a bounding box sprite or texture here:
    public Sprite borderSprite;
    public Texture2D borderTexture;
    // Link to the font for the labels:
    public Font font;

    const BackendType backend = BackendType.GPUCompute;
    private TranslationManager translationManager;

    private Transform displayLocation;
    private IWorker engine;
    private string[] labels;
    private RenderTexture targetRT;

    //Image size for the model
    private const int imageWidth = 640;
    private const int imageHeight = 640;

    private int numClasses;
    private VideoPlayer video;
    private Texture2D imageTexture;

    List<GameObject> boxPool = new();

    [SerializeField, Range(0, 1)] float iouThreshold = 0.5f;
    [SerializeField, Range(0, 1)] float scoreThreshold = 0.5f;
    int maxOutputBoxes = 64;

    TensorFloat centersToCorners;
    //bounding box data
    public struct BoundingBox
    {
        public float centerX;
        public float centerY;
        public float width;
        public float height;
        public string label;
    }


    public void Start()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        typeof(SimpleFileBrowser.FileBrowserHelpers).GetField("m_shouldUseSAF", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, (bool?)false);
#endif
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        //Parse neural net labels
        labels = labelsAsset.text.Split('\n');
        numClasses = labels.Length;

        if (translationJsonFile != null)
        {
            translationManager = new TranslationManager(translationJsonFile);
        }

        LoadModel();

        targetRT = new RenderTexture(imageWidth, imageHeight, 0);

        //Create image to display video
        displayLocation = displayImage.transform;

        if (borderSprite == null)
        {
            borderSprite = Sprite.Create(borderTexture, new Rect(0, 0, borderTexture.width, borderTexture.height), new Vector2(borderTexture.width / 2, borderTexture.height / 2));
        }

        FileBrowser.SetFilters(false, new FileBrowser.Filter("Videos", ".mp4"), new FileBrowser.Filter("Images", ".jpg", ".jpeg", ".png"));
        FileBrowser.SetDefaultFilter(".mp4");
        //FileBrowser.AddQuickLink("Videos", Application.streamingAssetsPath, null);
    }

    private IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, string.Empty, "Load");
        if (FileBrowser.Success)
        {
            OnFileSelected(FileBrowser.Result[0]);
        }
    }

    private void OnFileSelected(string path)
    {
        string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(path));
        FileBrowserHelpers.CopyFile(path, destinationPath);
        SetupInput(path);
    }

    private void LoadModel()
    {
        var model1 = ModelLoader.Load(asset);

        centersToCorners = new TensorFloat(new TensorShape(4, 4),
        new float[]
        {
                    1,      0,      1,      0,
                    0,      1,      0,      1,
                    -0.5f,  0,      0.5f,   0,
                    0,      -0.5f,  0,      0.5f
        });

        //Here we transform the output of the model1 by feeding it through a Non-Max-Suppression layer.
        var model2 = FF.Compile(
               input =>
               {
                   var modelOutput = model1.Forward(input)[0];
                   var boxCoords = modelOutput[0, 0..4, ..].Transpose(0, 1);        //shape=(8400,4)
                   var allScores = modelOutput[0, 4.., ..];                         //shape=(80,8400)
                   var scores = FF.ReduceMax(allScores, 0) - scoreThreshold;        //shape=(8400)
                   var classIDs = FF.ArgMax(allScores, 0);                          //shape=(8400) 
                   var boxCorners = FF.MatMul(boxCoords, FunctionalTensor.FromTensor(centersToCorners));
                   var indices = FF.NMS(boxCorners, scores, iouThreshold);           //shape=(N)
                   var indices2 = indices.Unsqueeze(-1).BroadcastTo(new int[] { 4 });//shape=(N,4)
                   var coords = boxCoords.Gather(0, indices2);                  //shape=(N,4)
                   var labelIDs = classIDs.Gather(0, indices);                  //shape=(N)
                   return (coords, labelIDs);
               },
               InputDef.FromModel(model1)[0]
         );

        //Create engine to run model
        engine = WorkerFactory.CreateWorker(backend, model2);
    }

    private void SetupInput(string path)
    {
        ClearInputs();
        string extension = Path.GetExtension(path).ToLower();
        switch (extension)
        {
            case ".mp4":
                video = gameObject.AddComponent<VideoPlayer>();
                video.renderMode = VideoRenderMode.APIOnly;
                video.source = VideoSource.Url;
                video.url = path;
                video.isLooping = true;
                video.Prepare();
                video.prepareCompleted += OnVideoPrepared;
                break;
            case ".jpg":
            case ".jpeg":
            case ".png":
            {
                byte[] imageData = File.ReadAllBytes(path);
                imageTexture = new Texture2D(2, 2);
                imageTexture.LoadImage(imageData);
                OnImagePrepared();
                break;
            }
            default:
                Debug.LogError("Unsupported file format: " + extension);
                break;
        }
    }

    private void ClearInputs()
    {
        if (video)
        {
            video.Stop();
            Destroy(video);
        }

        if (imageTexture)
        {
            Destroy(imageTexture);
        }
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        // Adjust the RenderTexture size to match the video's aspect ratio
        float aspectRatio = video.width * 1f / video.height;
        int newWidth = imageWidth;
        int newHeight = Mathf.RoundToInt(imageWidth / aspectRatio);
        targetRT.Release();
        targetRT = new RenderTexture(newWidth, newHeight, 0);

        displayImage.texture = targetRT;
        displayImage.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
        video.Play();
    }

    private void OnImagePrepared()
    {
        float aspectRatio = imageTexture.width * 1f / imageTexture.height;
        int newWidth = imageWidth;
        int newHeight = Mathf.RoundToInt(imageWidth / aspectRatio);
        targetRT.Release();
        targetRT = new RenderTexture(newWidth, newHeight, 0);

        displayImage.texture = targetRT;
        displayImage.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
        Graphics.Blit(imageTexture, targetRT);
    }

    private void Update()
    {
        ExecuteML();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            StartCoroutine(ShowLoadDialogCoroutine());
        }
    }

    //public void OpenFilePicker()
    //{
    //    var permission = NativeFilePicker.PickFile((path) =>
    //    {
    //        if (string.IsNullOrEmpty(path))
    //        {
    //            Debug.LogError("Invalid path");
    //            return;
    //        }

    //        Debug.Log("Picked file: " + path);
    //        SetupInput(path);
    //    }, new string[] { "image/*", "video/*" });

    //    Debug.Log("Permission: " + permission);
    //}

    public void ExecuteML()
    {
        ClearAnnotations();

        if (video && video.texture)
        {
            Graphics.Blit(video.texture, targetRT);
        }
        else if (imageTexture)
        {
            Graphics.Blit(imageTexture, targetRT);
        }
        else return;

        using var input = TextureConverter.ToTensor(targetRT, imageWidth, imageHeight, 3);
        engine.Execute(input);

        var output = engine.PeekOutput("output_0") as TensorFloat;
        var labelIDs = engine.PeekOutput("output_1") as TensorInt;

        output.CompleteOperationsAndDownload();
        labelIDs.CompleteOperationsAndDownload();

        float displayWidth = displayImage.rectTransform.rect.width;
        float displayHeight = displayImage.rectTransform.rect.height;

        float scaleX = displayWidth / imageWidth;
        float scaleY = displayHeight / imageHeight;

        int boxesFound = output.shape[0];
        //Draw the bounding boxes
        for (int n = 0; n < Mathf.Min(boxesFound, maxOutputBoxes); n++)
        {
            string trimmedLabel = labels[labelIDs[n]].Trim();
            string translation = translationManager?.GetTranslation(trimmedLabel, "polish");
            string labelWithTranslation = string.IsNullOrEmpty(translation) ? trimmedLabel : trimmedLabel + " (" + translation + ")";
            var box = new BoundingBox
            {
                centerX = output[n, 0] * scaleX - displayWidth / 2,
                centerY = output[n, 1] * scaleY - displayHeight / 2,
                width = output[n, 2] * scaleX,
                height = output[n, 3] * scaleY,
                label = labelWithTranslation
            };
            DrawBox(box, n, displayHeight * 0.05f);
        }
    }

    public void DrawBox(BoundingBox box, int id, float fontSize)
    {
        //Create the bounding box graphic or get from pool
        GameObject panel;
        if (id < boxPool.Count)
        {
            panel = boxPool[id];
            panel.SetActive(true);
        }
        else
        {
            panel = CreateNewBox(Color.yellow);
        }
        //Set box position
        panel.transform.localPosition = new Vector3(box.centerX, -box.centerY);

        //Set box size
        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(box.width, box.height);

        //Set label text
        var label = panel.GetComponentInChildren<Text>();
        label.text = box.label;
        label.fontSize = (int)fontSize;
    }

    public GameObject CreateNewBox(Color color)
    {
        //Create the box and set image

        var panel = new GameObject("ObjectBox");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.color = color;
        img.sprite = borderSprite;
        img.type = Image.Type.Sliced;
        panel.transform.SetParent(displayLocation, false);

        //Create the label

        var text = new GameObject("ObjectLabel");
        text.AddComponent<CanvasRenderer>();
        text.transform.SetParent(panel.transform, false);
        Text txt = text.AddComponent<Text>();
        txt.font = font;
        txt.color = color;
        txt.fontSize = 40;
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;

        RectTransform rt2 = text.GetComponent<RectTransform>();
        rt2.offsetMin = new Vector2(20, rt2.offsetMin.y);
        rt2.offsetMax = new Vector2(0, rt2.offsetMax.y);
        rt2.offsetMin = new Vector2(rt2.offsetMin.x, 0);
        rt2.offsetMax = new Vector2(rt2.offsetMax.x, 30);
        rt2.anchorMin = new Vector2(0, 0);
        rt2.anchorMax = new Vector2(1, 1);

        boxPool.Add(panel);
        return panel;
    }

    public void ClearAnnotations()
    {
        foreach (var box in boxPool)
        {
            box.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        centersToCorners?.Dispose();
        engine?.Dispose();
    }
}