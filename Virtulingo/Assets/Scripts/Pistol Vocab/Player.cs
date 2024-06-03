using UnityEngine;

namespace Pistol_Vocab
{
    public class Player : MonoBehaviour
    {
        public int score = 0;
        [SerializeField]
        private GameObject projectilePrefab;
        [SerializeField]
        private Transform gunTip;

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        void Shoot()
        {
            GameObject projectile = Instantiate(projectilePrefab, gunTip.position, gunTip.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = gunTip.forward * 10f;  // Adjust the speed as necessary

            // Play shooting sound or animation here if needed
        }

        public void AddScore(int points)
        {
            score += points;
        }
    }
}