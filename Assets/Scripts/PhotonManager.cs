using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonManager : MonoBehaviourPunCallbacks
{



    [SerializeField] private GameObject[] _spawnPoints;
    private PhotonView _Pvview;
    public GameObject _avatar;
    Player[] _player;
    private int _Currentclient;
   
    private void Awake()
    {
        GetSpawnPoints();
     
    }
    private void Start()
    {
        _Pvview = GetComponent<PhotonView>();
        _player = PhotonNetwork.PlayerList;
       //   if(GameManager.instance._competitive == false){
        foreach(Player p in _player)
        {
            if(p  != PhotonNetwork.LocalPlayer)
            {
                _Currentclient++;
            }
        }
        //  }

       
        if (_Pvview.IsMine)
        {
          
             // if(GameManager.instance._competitive == true){
            _Currentclient = PhotonNetwork.CurrentRoom.PlayerCount;
            _avatar = PhotonNetwork.Instantiate("Player", _spawnPoints[_Currentclient].transform.position, _spawnPoints[_Currentclient].transform.rotation,0);
            //  }else
             //   {
              
           // _avatar = PhotonNetwork.Instantiate("Player", _spawnPoints[_Currentclient].transform.position, _spawnPoints[_Currentclient].transform.rotation,0);

           }
        
           

        
    }
    private void GetSpawnPoints()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");


    }



}
