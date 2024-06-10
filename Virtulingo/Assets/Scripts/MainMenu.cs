using DG.Tweening;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IContent
{
    [SerializeField] private CanvasGroup cgMenu,htpButton;
    [SerializeField] private RectTransform exitTransform, titleTransform;
    [SerializeField] private RawImage instructions;
    
    private void Start()
    {
        cgMenu.DOFade(1.0f, 0.3f);
    }
    
    public void Disable(Action onComplete)
    {
        cgMenu.DOFade(0f, 1.0f).onComplete = () => {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        };
    }

    public void ShowInstructions()
    {
        htpButton.DOFade(0f, 0.3f).onComplete = () =>
            {
                Destroy(htpButton.gameObject);
                instructions.DOFade(1.0f, 0.5f);
            };
        exitTransform.DOAnchorPosY(240f, 0.5f);
        titleTransform.DOAnchorPosY(-240f, 0.5f);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
