using System;
using DG.Tweening;
using UnityEngine;

public class MemoMingle : MonoBehaviour, IContent
{
    [SerializeField] private CanvasGroup cgMemoMingle;

    private void Start()
    {
        cgMemoMingle.DOFade(1.0f, 0.75f);
    }

    public void Disable(Action onComplete)
    {
        cgMemoMingle.DOFade(0f, 1.0f).onComplete = () =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        };
    }
}
