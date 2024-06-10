using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject blackboard;
    void Start()
    {
        blackboard.SetActive(true);
    }
}