using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks,IPunObservable
{



    [SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();
    Player[] _player;
    private PhotonView _pv;
    private GameManager _gameManager;
    private Player _myplayer;
    public GameObject _mytotalscore;
  [SerializeField]  private GameObject totalscorePref;
     public GameObject _myavatar;
     [SerializeField] private Text _myTextScore;
     [SerializeField] private int myscore;
     [SerializeField] private List<GameObject> _totalScoretexts = new List<GameObject>();
     
     [SerializeField] private GameObject _speakingobj;
     [SerializeField] private GameObject _notspeakingobj;

    void Awake(){
   

     _player = PhotonNetwork.PlayerList;
        for(int i =0; i< _player.Length;i++){

            if(_player[i].IsLocal){

                _myplayer = _player[i];
            }
        }
        _pv = GetComponent<PhotonView>();
        _gameManager = FindObjectOfType<GameManager>();
      //  _Currentclient =1;
        GetSpawnPoints();
       
    }
    private void Start()
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
            _myavatar =    PhotonNetwork.Instantiate(PlayerPrefs.GetString("character", "Paul"),_spawnPoints[_myplayer.ActorNumber].transform.position,Quaternion.Euler(0,180,0),0);
              _mytotalscore = GameObject.FindWithTag("totalscorecanavas");
              _myavatar.GetComponent<PlayerController>()._myManager = this.gameObject;
              RpcSharetotalScore();
       
        }
     
    }

    private void RpcSharetotalScore(){

       totalscorePref = PhotonNetwork.Instantiate("_mytotalscoreprefab",_mytotalscore.transform.position,Quaternion.identity,0);
      _myavatar.GetComponent<PlayerController>()._mytotal = totalscorePref;
     
        _myTextScore =  totalscorePref.GetComponentInChildren<Text>();
  /*   foreach(Transform obj in totalscorePref.GetComponentsInChildren<Transform>()){

      /*   if(obj.gameObject.name == "speaking"){
           
            _myavatar.GetComponent<PlayerController>()._isspeakingButt = obj.gameObject;
        }
        if(obj.name == "Notspeaking"){
           
             _myavatar.GetComponent<PlayerController>()._notSpeakingButt = obj.gameObject;
        } */
    
  //  } 
     
        
        
    }
    void Update(){
        if(_pv.IsMine){
            if(_myavatar !=null){
            if( _myavatar.GetComponent<PlayerController>()._calcScore ==true){
                myscore = _myavatar.GetComponent<PlayerController>()._scoreplayer.totalscre;
                photonView.RPC("RpcTest",RpcTarget.All,myscore.ToString());
                _myavatar.GetComponent<PlayerController>()._calcScore =false;
            }
            }else{
                PhotonNetwork.Destroy(this.gameObject);
            }
            
        }
    }
    [PunRPC]
    private void RpcTest(string usedString){
     if(PhotonNetwork.OfflineMode == true){
          _totalScoretexts[0].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
     }else{
      
           _totalScoretexts[_pv.Owner.ActorNumber - 1].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
     }
    }
    private void GetSpawnPoints(){
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("spawnpoint")){

            _spawnPoints.Add(obj);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      
    }
   
}
