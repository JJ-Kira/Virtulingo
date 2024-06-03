using UnityEngine;

namespace Pistol_Vocab
{
    public class Blackboard : MonoBehaviour
    {
        public Question[] Questions;
        private int currentQuestionIndex = 0;

        public void DisplayQuestion()
        {
            if (currentQuestionIndex < Questions.Length)
            {
                // Code to display question and answers on the blackboard
                Question question = Questions[currentQuestionIndex];
                Debug.Log("Question: " + question.QuestionText);
                for (int i = 0; i < question.Answers.Length; i++)
                {
                    Debug.Log("Answer " + (i + 1) + ": " + question.Answers[i].answerText);
                }
            }
            else
            {
                Debug.Log("No more questions.");
            }
        }

        public void CheckAnswer(int answerIndex)
        {
            if (Questions[currentQuestionIndex].Answers[answerIndex].isCorrect)
            {
                FindObjectOfType<Player>().AddScore(10);  // Adjust the score increment as necessary
                Debug.Log("Correct Answer!");
            }
            else
            {
                Debug.Log("Wrong Answer!");
            }
            currentQuestionIndex++;
            DisplayQuestion();
        }
    }
}