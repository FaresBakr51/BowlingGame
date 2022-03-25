using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigRookGames.Weapons
{
    public class ProjectileController : MonoBehaviour
    {
        // --- Config ---
        public float speed = 100;
        public LayerMask collisionLayerMask;

        // --- Explosion VFX ---
        public GameObject rocketExplosion;

        // --- Projectile Mesh ---
        public MeshRenderer projectileMesh;

        // --- Script Variables ---
        private bool targetHit;

        // --- Audio ---
        public AudioSource inFlightAudioSource;

        // --- VFX ---
        public ParticleSystem disableOnHit;


        private void Update()
        {
            // --- Check to see if the target has been hit. We don't want to update the position if the target was hit ---
            if (targetHit) return;

            // --- moves the game object in the forward direction at the defined speed ---
            transform.position += transform.forward * (speed * Time.deltaTime);
        }


        /// <summary>
        /// Explodes on contact.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("pin"))
            {
                //collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 100, ForceMode.Impulse);
                //collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 3, ForceMode.Impulse);
                Explode();
                projectileMesh.enabled = false;
                targetHit = true;
                inFlightAudioSource.Stop();
                disableOnHit.Stop();
                
            }
            // --- return if not enabled because OnCollision is still called if compoenent is disabled ---
            if (!enabled) return;
            Destroy(gameObject, 5f);
        }


        /// <summary>
        /// Instantiates an explode object.
        /// </summary>
        private void Explode()
        {
            // --- Instantiate new explosion option. I would recommend using an object pool ---
            GameObject newExplosion = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);


        }
    }
}