using DG.Tweening;
using UnityEngine;
using System;

public class MainMenu : MonoBehaviour, IContent
{
    [SerializeField] private CanvasGroup cgMenu;
    
    private void Start()
    {
        cgMenu.DOFade(1.0f, 0.75f);
    }
    
    public void Disable(Action onComplete)
    {
        cgMenu.DOFade(0f, 1.0f).onComplete = () => {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        };
    }
}
