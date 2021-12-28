using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using TMPro;
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
  //   [SerializeField] GameObject _mytotal;
     public GameObject _myavatar;
     [SerializeField] private Text _myTextScore;
     [SerializeField] private int myscore;
     [SerializeField] private List<GameObject> _totalScoretexts = new List<GameObject>();
     [SerializeField] private int _mynumb;
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
        //    Debug.Log(_myplayer.ActorNumber);
           switch(PlayerPrefs.GetInt("character")){

               case 0:
                  _myavatar =    PhotonNetwork.Instantiate("Player2",_spawnPoints[_myplayer.ActorNumber].transform.position,Quaternion.Euler(0,180,0),0);


               break;
               case 1:
                  _myavatar =    PhotonNetwork.Instantiate("Player",_spawnPoints[_myplayer.ActorNumber].transform.position,Quaternion.Euler(0,180,0),0);

               break;
           }
              _mytotalscore = GameObject.FindWithTag("totalscorecanavas");
            
              RpcSharetotalScore();
       
        }
     
    }

    private void RpcSharetotalScore(){

       totalscorePref = PhotonNetwork.Instantiate("_mytotalscoreprefab",_mytotalscore.transform.position,Quaternion.identity,0);
      _myavatar.GetComponent<PlayerController>()._mytotal = totalscorePref;
        _myTextScore =  totalscorePref.GetComponentInChildren<Text>();
     
        
        
    }
    void Update(){
        if(_pv.IsMine){
            if( _myavatar.GetComponent<PlayerController>()._calcScore ==true){
                myscore = _myavatar.GetComponent<PlayerController>()._scoreplayer.totalscre;
                photonView.RPC("RpcTest",RpcTarget.All,myscore.ToString());
                _myavatar.GetComponent<PlayerController>()._calcScore =false;
            }
             
        }
    }
    [PunRPC]
    private void RpcTest(string usedString){
  
      Debug.Log(_pv.Owner.ActorNumber);
   /*    if(_pv.Owner.ActorNumber  == 1){
    
    _totalScoretexts[0].GetComponentInChildren<Text>().text = usedString;
      }else if (_pv.Owner.ActorNumber == 2){
           */
       _totalScoretexts[_pv.Owner.ActorNumber - 1].GetComponentInChildren<Text>().text = GetNickname.nickname + ": " + usedString;
          
   //   }
    }
    private void GetSpawnPoints(){
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("spawnpoint")){

            _spawnPoints.Add(obj);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    /*    if(stream.IsWriting){
           stream.SendNext(myscore);
           stream.SendNext(_myTextScore.text);

       }else{
           myscore = (int)stream.ReceiveNext();
           _myTextScore.text = (string)stream.ReceiveNext();

       } */
    }
}
