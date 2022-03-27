using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks,IPunObservable
{



    [SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();
    Player[] _players;
    private PhotonView _pv;
    private GameManager _gameManager;
    private Player _myplayer;
    public GameObject _mytotalscore;
    [SerializeField]  private GameObject totalscorePref;
    public GameObject _myavatar;
    [SerializeField] private int myscore;
    [SerializeField] private List<GameObject> _totalScoretexts = new List<GameObject>(); 
    [SerializeField] private GameObject _speakingobj;
    [SerializeField] private GameObject _notspeakingobj;
    public static int localPlayerIndex;
    private int _totalindex;
    public override void OnEnable()
    {
        _pv = GetComponent<PhotonView>();
        _players = PhotonNetwork.PlayerList;
        for (int i = 0; i < _players.Length; i++)
        {

            if (_players[i].IsLocal)
            {

                _myplayer = _players[i];
            }
        }
       
      
        _gameManager = FindObjectOfType<GameManager>();
         GetSpawnPoints();
        GetMyPoint();
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
            
              _mytotalscore = GameObject.FindWithTag("totalscorecanavas");
              _myavatar.GetComponent<PlayerController>()._myManager = this.gameObject;
        
             RpcSharetotalScore();
       
        }
     
    }

    private void GetMyPoint()
    {
        if (_pv.IsMine)
        {
          
                Debug.Log(_pv.Owner.ActorNumber);
                if (_myplayer.ActorNumber == 1)
                {
                    localPlayerIndex = 0;
                }
                else if (_myplayer.ActorNumber == 2)
                {
                    localPlayerIndex = 1;
                }
                else if (_myplayer.ActorNumber == 3)
                {
                    localPlayerIndex = 2;
                }
                else if (_myplayer.ActorNumber == 4)
                {
                    localPlayerIndex = 3;
                }
                else if (_myplayer.ActorNumber == 5)
                {
                    localPlayerIndex = 4;
                }
                else if (_myplayer.ActorNumber == 6)
                {
                    localPlayerIndex = 5;
                }
                else if (_myplayer.ActorNumber == 7)
                {
                    localPlayerIndex = 6;
                }
                else if (_myplayer.ActorNumber == 8)
                {
                    localPlayerIndex = 7;
                }
            
        }


    }
   
    private void RpcSharetotalScore(){

       totalscorePref = PhotonNetwork.Instantiate("_mytotalscoreprefab",_mytotalscore.transform.position,Quaternion.identity,0);
      _myavatar.GetComponent<PlayerController>()._mytotal = totalscorePref;
   
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
                  photonView.RPC("RpcShareScore", RpcTarget.All,myscore.ToString());
                
                _myavatar.GetComponent<PlayerController>()._calcScore =false;
            }
            }else{
                 
                PhotonNetwork.Destroy(this.gameObject);
            }

    }
    }
    [PunRPC]
    private void RpcShareScore(string usedString){

        Debug.Log("score shared");
        //if(PhotonNetwork.OfflineMode){
        //     _totalScoretexts[0].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        //}else{

        //if (_totalScoretexts[localPlayerIndex].gameObject != null)
        //{
        //  _totalScoretexts[localPlayerIndex].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        //  totalscorePref.GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        // }
        //if (!GameManager.instance._rankedMode) {
        //if (!GameManager.instance._rankedMode)
        //{

       
        if (_totalScoretexts[_pv.Owner.ActorNumber - 1].gameObject == null) return;
           _totalScoretexts[_pv.Owner.ActorNumber - 1].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
          

        //if (_pv.IsMine)
        //{
        //    _totalScoretexts[_myplayer.ActorNumber - 1].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        //}
        //else
        //{
        //    _totalScoretexts[localPlayerIndex++].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        //}
        //  }
        //else
        //{
        //    if (_pv.IsMine)
        //    {
        //        _totalScoretexts[0].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        //    }
        //    else
        //    {
        //        _totalScoretexts[1].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        //    }
        //}
        //if (_totalScoretexts[localPlayerIndex].gameObject != null)
        //{
        //    totalscorePref.GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        //    _totalScoretexts[localPlayerIndex].GetComponentInChildren<Text>().text = _pv.Owner.NickName + ": " + usedString;
        //}

        //else
        //{


        //}

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
