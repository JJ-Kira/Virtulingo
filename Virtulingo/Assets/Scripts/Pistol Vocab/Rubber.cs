using UnityEngine;

namespace Pistol_Vocab
{
    [RequireComponent(typeof(Rigidbody))] 
    [RequireComponent(typeof(Collider))] 
    public class Rubber : MonoBehaviour
    {
        void Start()
        {
            // Ensure the projectile has a Rigidbody and a Collider
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            Collider rubberCollider = gameObject.GetComponent<Collider>();
                if (rubberCollider == null)
            {
                gameObject.AddComponent<SphereCollider>(); // Or another collider type
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            // Handle collision with targets or other objects
            if (collision.gameObject.CompareTag("Target"))
            {
                // Additional logic if needed
            }
            
            // Destroy the projectile after it hits something
            Destroy(gameObject);
        }
    }
}