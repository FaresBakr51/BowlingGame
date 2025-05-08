using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class BallSound : MonoBehaviourPunCallbacks,IPunObservable
{
    public AudioSource _audiosource;
    public AudioClip _clip;
    public bool _hit;
    private Rigidbody _rig;
    Vector3 _netpos;
    Quaternion _netrot;
    [SerializeField] private TextMeshProUGUI ping;
    [SerializeField] private float RTT;

    /**
     * lag compensation
     * */

    Vector3 latestPos;
    Quaternion latestRot;
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;
    public float teleportIfFarDistance;
    public bool Collided = false;
    private Vector3 cachedRot;
    private Vector3 rightAxis;
    void Awake()

    {
      
        _rig = GetComponent<Rigidbody>();

        transform.rotation = Quaternion.identity;
        transform.right = Vector3.right;
        rightAxis = transform.right;
    }
   
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pin") && _hit == false)
        {
            UpdateSound(_clip);
            _hit = true;
          
        }
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("pin"))
        {
            cachedRot = transform.eulerAngles;
            Collided = true;
        }
       

    }
    public Vector3 GetRightAXIS()
    {
        return rightAxis;
    }
  void FixedUpdate()
    {
        if(!PhotonNetwork.OfflineMode){ 
      if (!photonView.IsMine)
            {
                double timetoreachgoal = currentPacketTime - lastPacketTime;
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(positionAtLastPacket, latestPos,(float)(currentTime/timetoreachgoal));
                transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot,  (float)(currentTime / timetoreachgoal));

                if (Vector3.Distance(transform.position, latestPos) > teleportIfFarDistance)
                {
                    transform.position = latestPos;
                }
            }
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
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            latestPos = (Vector3)stream.ReceiveNext();//recived pos
            latestRot = (Quaternion)stream.ReceiveNext();//recived rot
          //  print("Current  Pos = " + transform.position + "  recived Pos = " + latestPos);
           
            //data recived
            currentTime = 0;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;//time the messeage gets from server
            positionAtLastPacket = transform.position;//get recived pos at packet time
            rotationAtLastPacket = transform.rotation;//get recived Rot at packet time
        }

    }
}
