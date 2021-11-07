using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BallSound : MonoBehaviourPunCallbacks,IPunObservable
{
    public AudioSource _audiosource;
    public AudioClip _clip;
    public bool _hit;
    private Rigidbody _rig;
    private PhotonView _pv;
    void Awake()

    {
        _pv = GetComponent<PhotonView>();
        _rig = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pin") && _hit == false)
        {
            UpdateSound(_clip);
            _hit = true;
        }
    }
  void Update()
    {

    }
   
    public void UpdateSound(AudioClip _clip)
    {
        _audiosource.PlayOneShot(_clip);
    }
  
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(_rig.velocity);
             stream.SendNext(_rig.angularVelocity);
        }
        else if (stream.IsReading)
        {

            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            _rig.velocity = (Vector3)stream.ReceiveNext();
            _rig.angularVelocity = (Vector3)stream.ReceiveNext();
        }

    }
}
