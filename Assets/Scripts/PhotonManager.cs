using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonManager : MonoBehaviourPunCallbacks
{



    [SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();
    Player[] _player;
    [SerializeField]   private int _Currentclient = 1;
    private PhotonView _pv;
    private bool StartGame = true;
    private int index;
    private List<GameObject> _playerscount = new List<GameObject>();
    private GameManager _gameManager;
    private Player _myplayer;
    public GameObject _mytotalscore;
     [SerializeField] GameObject _mytotal;
     public GameObject _myavatar;
    void Awake(){
   

     _player = PhotonNetwork.PlayerList;
        for(int i =0; i< _player.Length;i++){

            if(_player[i].IsLocal){

                _myplayer = _player[i];
            }
        }
        _pv = GetComponent<PhotonView>();
        _gameManager = FindObjectOfType<GameManager>();
        StartGame = true;
      //  _Currentclient =1;
        GetSpawnPoints();
    }
    private void Start()
    {
        
        _Currentclient = PhotonNetwork.PlayerList.Length;
        Debug.Log(_Currentclient);
        if(_pv.IsMine){
            Debug.Log(_myplayer.ActorNumber);
           switch(PlayerPrefs.GetInt("character")){

               case 0:
                  _myavatar =    PhotonNetwork.Instantiate("Player2",_spawnPoints[_myplayer.ActorNumber].transform.position,Quaternion.Euler(0,180,0),0);


               break;
               case 1:
                  _myavatar =    PhotonNetwork.Instantiate("Player",_spawnPoints[_myplayer.ActorNumber].transform.position,Quaternion.Euler(0,180,0),0);

               break;
           }
      
              _mytotalscore = GameObject.FindWithTag("totalscorecanavas");
             _mytotal = PhotonNetwork.Instantiate("_mytotalscoreprefab",transform.position,Quaternion.identity,0);
              _mytotal.transform.parent = _mytotalscore.transform;
              _mytotal.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
              _myavatar.GetComponent<PlayerController>()._mytotal = _mytotal;
              
        }
    }

    private void GetSpawnPoints(){
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("spawnpoint")){

            _spawnPoints.Add(obj);
        }
    }
}
