using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Pistol_Vocab
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Answer: MonoBehaviour
    {
        public bool isCorrect;
        [HideInInspector] public PistolWhip pvm;

        [SerializeField] private TextMeshProUGUI text;
        
        void Start()
        {
            // Ensure the target has a Collider
            Collider collider = gameObject.GetComponent<Collider>();
            if (collider == null)
            {
                gameObject.AddComponent<BoxCollider>(); // Or another collider type
            }
        }
        
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                Destroy(collision.gameObject);  // Destroy the projectile
                if (isCorrect)
                {
                    pvm.AddScore();
                }
                else
                {
                    // note wrong answer and/or subtract points
                    pvm.SubtractScore();
                    text.DOFade(0.75f, 0.75f);
                }
            }
        }
    }
}