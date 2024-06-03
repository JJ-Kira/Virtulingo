using UnityEngine;

namespace Pistol_Vocab
{
    public class GameManager : MonoBehaviour
    {
        public Player player;
        public Blackboard blackboard;

        void Start()
        {
            blackboard.DisplayQuestion();
        }

        void Update()
        {
            // Here you can manage the game state, check for game over conditions, etc.
        }
    }
}