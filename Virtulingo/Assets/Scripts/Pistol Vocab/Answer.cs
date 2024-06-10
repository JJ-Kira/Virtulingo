using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Pistol_Vocab
{
    [RequireComponent(typeof(Collider))] 
    public class Answer: MonoBehaviour
    {
        public bool isCorrect;

        [HideInInspector] public PVManager pvm; 
        
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
            //TODO
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
                }
            }
        }
    }
}