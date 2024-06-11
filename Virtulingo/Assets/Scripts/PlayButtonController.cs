using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonController : MonoBehaviour
{
    public bool IsPressed = false;
    private bool animating = false;
    [SerializeField] private Animator animator;
    [SerializeField] private Blackboard blackboard;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!animating)
            {
                animating = true;
                if (!IsPressed)
                {
                    animator.Play("pushDown");
                    IsPressed = true;
                }
                else
                {
                    animator.Play("resetButton");
                    IsPressed = false;
                }
            }
        }
    }

    public void StartPistolVocab()
    {
        animating = false;
        blackboard.ChangeContent(ActiveContent.PistolVocab);
    }

    public void StopPistolVocab()
    {
        animating = false;
        blackboard.ChangeContent(ActiveContent.Menu);
    }

    public void StartMemoMingle()
    {
        animating = false;
        blackboard.ChangeContent(ActiveContent.MemoMingle);
    }

    public void StopMemoMingle()
    {
        animating = false;
        blackboard.ChangeContent(ActiveContent.Menu);
    }
}
