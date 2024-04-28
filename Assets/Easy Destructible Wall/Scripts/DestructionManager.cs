using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

namespace EasyDestuctibleWall {
    public class DestructionManager : MonoBehaviour, IPunObservable {
        // The hitpoints of the object, when this value is below 1, the chunk will fracture
        [SerializeField]
        private float health = 50f;

        // These two variables are used to multiply damage based on velocity and torque respectively.
        [SerializeField]
        private float impactMultiplier = 5.25f;
        [SerializeField]
        private float twistMultiplier = 0.0025f;
        [SerializeField] private PhotonView photonView;
        private Rigidbody cachedRigidbody;
        private List<Transform> removed = new List<Transform>();
        private void Awake() {
            cachedRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            /**
            * Damage based on torque. When an object spins very fast, it is expected that this force will
            * tear it apart
            */
            health -= Mathf.Round(cachedRigidbody.angularVelocity.sqrMagnitude * twistMultiplier);

            if (health <= 0f) {

                RPCTorque();
                //foreach(Transform child in transform) {
                //    Rigidbody spawnRB = child.gameObject.AddComponent<Rigidbody>();
                //    child.parent = null;
                //    // Transfer velocity
                //    spawnRB.velocity = GetComponent<Rigidbody>().GetPointVelocity(child.position);
                //    // Transfer torque
                //    spawnRB.AddTorque(GetComponent<Rigidbody>().angularVelocity, ForceMode.VelocityChange);
                //}
                //Destroy(gameObject); // Destroy this now empty chunk object
            }
        }

        // When the chunk hits another object, take some of its health away
        void OnCollisionEnter(Collision collision) {
            float relativeVelocity = collision.relativeVelocity.sqrMagnitude;

            // If the chunk was hit by a rigidbody, multiply the damage by its mass
            if (collision.gameObject.CompareTag("ball") || collision.gameObject.CompareTag("projectile"))
            {
                Debug.LogError("Collided with" + collision.gameObject.tag) ;


                if (!photonView.gameObject.GetComponent<Wall>().Destroyed)
                {
                    StartCoroutine(WaitDestroy());
                    photonView.gameObject.GetComponent<Wall>().Destroyed = true;
                }
            }
            if (collision.rigidbody && photonView)
            {
               // photonView.RPC("RPCSendHealth", RpcTarget.All, relativeVelocity, impactMultiplier, collision.rigidbody.mass);

                 health -= relativeVelocity * impactMultiplier * collision.rigidbody.mass;
            }
            else
            {
                health -= relativeVelocity * impactMultiplier;
                //if (photonView)
                //{
                //    photonView.RPC("RPCSendHealth", RpcTarget.All, relativeVelocity, impactMultiplier, 1);
                //    //  health -= relativeVelocity * impactMultiplier;
                //}
            }
        }
        IEnumerator WaitDestroy()
        {
            yield return new WaitForSeconds(3f);
            PhotonNetwork.Destroy(photonView.gameObject);
        }

       // [PunRPC]
        private void RPCTorque()
        {
            foreach (Transform child in transform)
            {
                Rigidbody spawnRB = child.gameObject.AddComponent<Rigidbody>();
                removed.Add(child);
                child.parent = null;
                
                // Transfer velocity
                spawnRB.velocity = GetComponent<Rigidbody>().GetPointVelocity(child.position);
                // Transfer torque
                spawnRB.AddTorque(GetComponent<Rigidbody>().angularVelocity, ForceMode.VelocityChange);
                Destroy(child.gameObject,3);
            }
            Debug.Log("DestroyChunk");
            Destroy(gameObject);
        }
        //[PunRPC]
        //private void RPCSendHealth(float relativevelocity,float impactpower,float collisionmass)
        //{
        //    health -= relativevelocity * impactpower * collisionmass;
        //}

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(health);
            }
            else
            {
                health = (float)stream.ReceiveNext();
                //if (health <= 0)
                //{
                //    foreach (Transform child in removed)
                //    {
                //        Destroy(child.gameObject);
                //    }
                //}
            }
        }
    }
}