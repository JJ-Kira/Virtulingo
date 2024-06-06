using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Blackboard : MonoBehaviour
{
    [SerializeField] private CanvasGroup menu, pistolWhip, memoMingle;
    private ActiveContent currentlyActiveContent = 0;
    public void ChangeContent(ActiveContent newContent)
    {
        currentlyActiveContent = newContent;
        //TODO
    }
}

public enum ActiveContent
{
    Menu,
    PistolWhip,
    MemoMingle
}