using UnityEngine;

namespace Pistol_Vocab
{
    [RequireComponent(typeof(Rigidbody))] 
    [RequireComponent(typeof(Collider))] 
    public class Rubber : MonoBehaviour
    {
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