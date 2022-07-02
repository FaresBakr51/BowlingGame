using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class PhotonManager : MonoBehaviourPunCallbacks,IPunObservable
{



    [SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();
    private PhotonView _pv;
   
    public GameObject _mytotalscore;
    [SerializeField]  private GameObject totalscorePref;
    public GameObject _myavatar;
    private PlayerController _myAvatarController;
    [SerializeField] private int myscore;
    [SerializeField] private List<GameObject> _totalScoretexts = new List<GameObject>(); 
    [SerializeField] private GameObject _speakingobj;
    [SerializeField] private GameObject _notspeakingobj;
    private bool _startCounter;
    public override void OnEnable()
    {
        _pv = GetComponent<PhotonView>();
         GetSpawnPoints();
      
    }


    private void  Start()
    {
    
        SetUpPlayer();
        StartCoroutine(_GETAlltotalscore());
    }

    
    IEnumerator _GETAlltotalscore(){
     
     
        yield return new WaitForSeconds(1.5f);
         foreach(GameObject obj in GameObject.FindGameObjectsWithTag("totalplayerscore")){
          _totalScoretexts.Add(obj);

      }
       


    }
  
    private void SetUpPlayer(){
        if(_pv.IsMine){
            
            _myavatar =    PhotonNetwork.Instantiate(PlayerPrefs.GetString("character", "Paul"),_spawnPoints[_pv.Owner.ActorNumber-1].transform.position,Quaternion.Euler(0,180,0),0);
            _myAvatarController = _myavatar.GetComponent<PlayerController>();
              _mytotalscore = GameObject.FindWithTag("totalscorecanavas");
            _myAvatarController._myManager = this.gameObject;
        
             RpcSharetotalScore();
       
        }
     
    }

   
   
    private void RpcSharetotalScore(){

       totalscorePref = PhotonNetwork.Instantiate("_mytotalscoreprefab",_mytotalscore.transform.position,Quaternion.identity,0);

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
              var platform = PhotonNetwork.Instantiate("platformpc", totalscorePref.transform.position, Quaternion.identity, 0);
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
        {
               var platform =   PhotonNetwork.Instantiate("platformarcade", totalscorePref.transform.position, Quaternion.identity, 0);
     
        }
        _myAvatarController._mytotal = totalscorePref;

        /*   foreach(Transform obj in totalscorePref.GetComponentsInChildren<Transform>()){

            /*   if(obj.gameObject.name == "speaking"){

                  _myavatar.GetComponent<PlayerController>()._isspeakingButt = obj.gameObject;
              }
              if(obj.name == "Notspeaking"){

                   _myavatar.GetComponent<PlayerController>()._notSpeakingButt = obj.gameObject;
              } */

        //  } 
        StartCoroutine(ShareName());


    }
    IEnumerator ShareName()
    {
        
        yield return new WaitForSeconds(2f);
       
        photonView.RPC("RpcShareName", RpcTarget.All);
        _startCounter = true;
       
    }
    void Update(){
        if(_pv.IsMine){
            if(_myavatar !=null){
                if (GameModes._battleRoyale)
                {
                    if (_myAvatarController._canhit)
                    {
                        if (_myAvatarController._timerAfk >= 0 && _startCounter)
                        {
                            
                            _totalScoretexts[_pv.OwnerActorNr - 1].GetComponentInChildren<Image>().GetComponentInChildren<TextMeshProUGUI>().text = ((int)_myAvatarController._timerAfk).ToString();
                            
                        }
                    }
                }
                else
                {

                    if (_myAvatarController._calcScore)
                    {
                        myscore = _myAvatarController._scoreplayer.totalscre;
                        if (!GameModes._battleRoyale)
                        {
                            photonView.RPC("RpcShareScore", RpcTarget.All, myscore.ToString(), _myAvatarController._scoreplayer._currentframe);
                        }
                        _myAvatarController._calcScore = false;
                    }
                }
            }else{
                 
                PhotonNetwork.Destroy(this.gameObject);
            }


        }
     
    }
   
    [PunRPC]
    private void RpcShareScore(string usedString, int framenumb){


        if (_totalScoretexts[_pv.OwnerActorNr - 1].gameObject == null) return;
           _totalScoretexts[_pv.OwnerActorNr - 1].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        if (framenumb <= 9)
        {
            _totalScoretexts[_pv.OwnerActorNr - 1].GetComponentInChildren<Image>().GetComponentInChildren<TextMeshProUGUI>().text = (framenumb + 1).ToString();
        }
    }
    [PunRPC]
    private void RpcShareName()
    {


        if (_totalScoretexts[_pv.OwnerActorNr - 1].gameObject == null) return;
        _totalScoretexts[_pv.OwnerActorNr - 1].GetComponentInChildren<Text>().text = _pv.Owner.NickName;
    }


    private void GetSpawnPoints(){
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("spawnpoint"))
        {

            _spawnPoints.Add(obj);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
     
    }
   
}
