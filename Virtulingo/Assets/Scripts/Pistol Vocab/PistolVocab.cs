using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace Pistol_Vocab
{
    public class PistolVocab : MonoBehaviour, IContent
    {
        private Question currentQuestion;

        [SerializeField] private TextMeshProUGUI question, congrats;
        [SerializeField] private TextMeshProUGUI[] answers;
        [SerializeField] private GameObject gunPrefabLeft;
        [SerializeField] private CanvasGroup pvCanvasGroup;
        [SerializeField] private GameObject[] originalControllers, gunControllers;

        private List<Question> questions = new List<Question>();
        
        public int score = 0;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform gunTipLeft, gunTipRight;
        [SerializeField] private float projectileSpeed = 10.0f;

        private void Awake()
        {
            questions.Add(new Question("He realised that he had to .... their trust.", new string[4] {"a. win", "catch", "achieve", "receive"}));
            questions.Add(new Question(".... to popular belief.", new string[4] {"Contrary", "Opposite", "Opposed", "Contradictory"}));
            questions.Add(new Question("He contends that bears do not .... as much for fruit as previously supposed.", new string[4] {"care", "bother", "desire", "hope"}));
            questions.Add(new Question("After devoting years of his life to bears, he is under no .... about their feelings for him.", new string[4] {"illusion", "error", "doubt", "impression"}));
            questions.Add(new Question("It is clear that their interest in him does not .... beyond the food he brings.", new string[4] {"extend", "expand", "widen", "spread"}));
            questions.Add(new Question("It is important to ... between a fear and a phobia.", new string[4] {"distinguish", "choose", "select", "pick"}));
        }

        void Start()
        {
            DrawAndDisplayNewQuestion();
            
            foreach (var controller in originalControllers)
                controller.SetActive(false);
            foreach (var controller in gunControllers)
                controller.SetActive(true);
        }

        public void Disable(Action onComplete)
        {
            foreach (var controller in originalControllers)
                controller.SetActive(true);
            foreach (var controller in gunControllers)
                controller.SetActive(false);
            
            pvCanvasGroup.DOFade(0f, 1.0f).onComplete = () => {
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
            pvCanvasGroup.DOFade(0f, 0.75f).onComplete = () =>
            {
                congrats.alpha = 0f;
                
                currentQuestion = questionToDisplay;
                question.text = currentQuestion.QuestionText;
                Debug.Log("Question: " + currentQuestion.QuestionText);
                pvCanvasGroup.DOFade(1.0f, 0.75f);
                
                var correctAnswer = currentQuestion.Answers.First();
                currentQuestion.Answers.Shuffle();
                for (int i = 0; i < currentQuestion.Answers.Count; i++)
                {
                    var currentAnswer = answers[i];
                    
                    currentAnswer.text = currentQuestion.Answers[i];
                    currentAnswer.alpha = 0f;
                    currentAnswer.DOFade(1.0f, 0.75f);

                    var ans = currentAnswer.GetComponent<Answer>();
                    ans.isCorrect = currentQuestion.Answers[i] == correctAnswer;
                    ans.pvm = this;
                    Debug.Log("Answer " + (i + 1) + ": " + currentQuestion.Answers[i]);
                }
            };
        }
        
        void Update()
        {
            OVRInput.Update();
        }

        private void LateUpdate()
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                Shoot(gunTipRight);
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                Shoot(gunTipLeft);
        }

        void Shoot(Transform gunTip)
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