using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Blackboard : MonoBehaviour
{
    [SerializeField] private GameObject[] contentGOs;
    private IContent[] contents = new IContent[3];
    private ActiveContent currentlyActiveContent = ActiveContent.Menu;

    private void Start()
    {
        for (int i = 0; i < contentGOs.Length; i++)
        {
            contents[i] = contentGOs[i].GetComponent(typeof(IContent)) as IContent;
            if (contents[i] == null)
            {
                Debug.LogError("GameObject " + contentGOs[i].name + " does not have a component that implements IContent.");
            }
        }
        contentGOs[(int) ActiveContent.PistolVocab].SetActive(false);
        contentGOs[(int) ActiveContent.MemoMingle].SetActive(false);
        contentGOs = null;
    }
    
    public void ChangeContent(ActiveContent newContent)
    {
        contents[(int)currentlyActiveContent].Disable(ActivateNewContent);
        currentlyActiveContent = newContent;
    }

    private void ActivateNewContent()
    {
        MonoBehaviour component = contents[(int)currentlyActiveContent] as MonoBehaviour;
        component.gameObject.SetActive(true);
    }
}

public enum ActiveContent
{
    Menu,
    PistolVocab,
    MemoMingle
}

interface IContent 
{
    public void Disable(Action onComplete);
}