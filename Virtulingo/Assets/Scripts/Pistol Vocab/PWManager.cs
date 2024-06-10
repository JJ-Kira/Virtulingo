using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Pistol_Vocab
{
    public class PWManager : MonoBehaviour, IContent
    {
        private Question currentQuestion;

        [SerializeField] private TextMeshProUGUI question, congrats;
        [SerializeField] private GameObject answerPrefab;
        [SerializeField] private CanvasGroup pwCanvasGroup;

        [CanBeNull] private List<Question> questions;
        
        public int score = 0;
        [SerializeField]
        private GameObject projectilePrefab;
        [SerializeField]
        private Transform gunTip;
        [SerializeField]
        private float projectileSpeed = 10f;

        private void Awake()
        {
            questions.Add(new Question("He realised that he had to .... their trust.", new string[4] {"win", "catch", "achieve", "receive"}));
            questions.Add(new Question(".... to popular belief.", new string[4] {"Contrary", "Opposite", "Opposed", "Contradictory"}));
            questions.Add(new Question("He contends that bears do not .... as much for fruit as previously supposed.", new string[4] {"care", "bother", "desire", "hope"}));
            questions.Add(new Question("After devoting years of his life to bears, he is under no .... about their feelings for him.", new string[4] {"illusion", "error", "doubt", "impression"}));
            questions.Add(new Question("It is clear that their interest in him does not .... beyond the food he brings.", new string[4] {"extend", "expand", "widen", "spread"}));
            questions.Add(new Question("It is important to ... between a fear and a phobia.", new string[4] {"distinguish", "choose", "select", "pick"}));
        }

        void Start()
        {
            //TODO: activate the gun
            DrawAndDisplayNewQuestion();
        }

        public void Disable(Action onComplete)
        {
            //TODO: deactivate the gun
            pwCanvasGroup.DOFade(0f, 1.0f).onComplete = () => {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            };
        }

        private void DrawAndDisplayNewQuestion()
        {
            //TODO (later): get new question from db
            DisplayQuestion(questions.PickRandom());
        }

        private void DisplayQuestion(Question questionToDisplay)
        {
            // tidy existing content
            pwCanvasGroup.DOFade(0f, 0.75f).onComplete = () =>
            {
                congrats.alpha = 0f;
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("Target"))
                    {
                        Destroy(child.gameObject);
                    }
                }
                
                currentQuestion = questionToDisplay;
                question.text = currentQuestion.QuestionText;
                Debug.Log("Question: " + currentQuestion.QuestionText);
                pwCanvasGroup.DOFade(1.0f, 0.75f);
                
                RectTransform canvasRectTransform = GetComponent<RectTransform>();
                // Calculate the spacing between each text element
                float verticalSpacing = canvasRectTransform.rect.height / (currentQuestion.Answers.Count + 1);
                float horizontalSpacing = canvasRectTransform.rect.width / (currentQuestion.Answers.Count + 1);

                var correctAnswer = currentQuestion.Answers.First();
                currentQuestion.Answers.Shuffle();
            
                for (int i = 0; i < currentQuestion.Answers.Count; i++)
                {
                    var currentAnswer = Instantiate(answerPrefab, transform);
                    var answerTransform = currentAnswer.GetComponent<RectTransform>();
                    float xPos = (i + 1) * horizontalSpacing - canvasRectTransform.rect.width / 2;
                    float yPos = -(i + 1) * verticalSpacing + canvasRectTransform.rect.height / 2;
                    answerTransform.anchoredPosition = new Vector2(xPos, yPos);
                    //answerTransform.sizeDelta = new Vector2(horizontalSpacing, answerTransform.sizeDelta.y);
                
                    TextMeshProUGUI textComponent = currentAnswer.GetComponent<TextMeshProUGUI>();
                    textComponent.text = currentQuestion.Answers[i];
                    textComponent.alpha = 0f;
                    textComponent.DOFade(1.0f, 0.75f);

                    var ans = currentAnswer.GetComponent<Answer>();
                    ans.isCorrect = currentQuestion.Answers[i] == correctAnswer;
                    ans.pvm = this;
                    Debug.Log("Answer " + (i + 1) + ": " + currentQuestion.Answers[i]);
                }
            };
        }
        
        void Update()
        {
            //TODO: check for trigger
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        void Shoot()
        {
            GameObject projectile = Instantiate(projectilePrefab, gunTip.position, gunTip.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = gunTip.forward * projectileSpeed;

            // Play shooting sound or animation here if needed
        }

        public void SubtractScore()
        {
            score -= 1;
        }
        
        public void AddScore()
        {
            score += currentQuestion.Answers.Count;
            congrats.DOFade(1.0f, 0.75f).onComplete = DrawAndDisplayNewQuestion;
        }
    }
    
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}