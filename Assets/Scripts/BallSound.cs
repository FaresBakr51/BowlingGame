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
    Vector3 _netpos;
    Quaternion _netrot;
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
      if (!photonView.IsMine)
    {
       transform.position = Vector3.Lerp(transform.position, _netpos,  0.5f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _netrot,720f * Time.deltaTime);
    }
    }
   
    public void UpdateSound(AudioClip _clip)
    {
        _audiosource.PlayOneShot(_clip);
    }
  
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
           stream.SendNext(this.transform.position);
        stream.SendNext(this.transform.rotation);
       // stream.SendNext(this._rig.velocity);
        }
        else if (stream.IsReading)
        {

               _netpos = (Vector3) stream.ReceiveNext();
        _netrot = (Quaternion) stream.ReceiveNext();
       /*  _rig.velocity = (Vector3) stream.ReceiveNext();
             float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp));
        _netpos += (this._rig.velocity * lag); */
        }

    }
}
