using UnityEngine;

namespace Pistol_Vocab
{
    public class PVManager : MonoBehaviour
    {
        public Player player;
        public Blackboard blackboard;
        
        void Start()
        {
            DrawAndDisplayNewQuestion();
        }

        public void DrawAndDisplayNewQuestion()
        {
            //TODO
            Question question = new Question();
            blackboard.DisplayQuestion(question);
        }
    }
}