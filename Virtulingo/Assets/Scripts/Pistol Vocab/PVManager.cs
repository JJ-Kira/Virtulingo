using TMPro;
using UnityEngine;

namespace Pistol_Vocab
{
    public class PVManager : MonoBehaviour
    {
        void Start()
        {
            DrawAndDisplayNewQuestion();
        }

        public void DrawAndDisplayNewQuestion()
        {
            //TODO
            Question question = new Question();
            DisplayQuestion(question);
        }
        
        private Question currentQuestion;

        [SerializeField] private TextMeshProUGUI question;
        [SerializeField] private GameObject answerPrefab;

        public void DisplayQuestion(Question questionToDisplay)
        {
            currentQuestion = questionToDisplay;
            question.text = currentQuestion.QuestionText;
            Debug.Log("Question: " + currentQuestion.QuestionText);
                
            RectTransform canvasRectTransform = GetComponent<RectTransform>();
            // Calculate the spacing between each text element
            float verticalSpacing = canvasRectTransform.rect.height / (currentQuestion.Answers.Length + 1);
            float horizontalSpacing = canvasRectTransform.rect.width / (currentQuestion.Answers.Length + 1);
            for (int i = 0; i < currentQuestion.Answers.Length; i++)
            {
                var currentAnswer = Instantiate(answerPrefab, transform);
                var answerTransform = currentAnswer.GetComponent<RectTransform>();
                float xPos = (i + 1) * horizontalSpacing - canvasRectTransform.rect.width / 2;
                float yPos = -(i + 1) * verticalSpacing + canvasRectTransform.rect.height / 2;
                answerTransform.anchoredPosition = new Vector2(xPos, yPos);
                //answerTransform.sizeDelta = new Vector2(horizontalSpacing, answerTransform.sizeDelta.y);
                
                TextMeshProUGUI textComponent = currentAnswer.GetComponent<TextMeshProUGUI>();
                textComponent.text = currentQuestion.Answers[i].answerText;
                Debug.Log("Answer " + (i + 1) + ": " + currentQuestion.Answers[i].answerText);
            }
        }

        public void CheckAnswer()
        {
            //TODO
        }
    }
}