using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
public class OfflinePlayerMode : MonoBehaviour
{
  [SerializeField] private Transform[] _spawnPoints;
  [SerializeField] private GameObject[] _playersObj;
   [SerializeField] private GameObject[] _Cameras;
    [SerializeField] private GameObject[] offlineModeobj;
 public GameObject[] _scoreBoard;
  public List<GameObject> _controllcanavas = new List<GameObject>();
 public List<GameObject> _CurrentPlayers = new List<GameObject>();
    void Awake()
    {
      PhotonNetwork.OfflineMode = true;
      if(PhotonNetwork.OfflineMode == true &&PhotonNetwork.InRoom == true){

          PhotonNetwork.Instantiate("PhotonNetworkAvatar",transform.position,transform.rotation, 0);
      }
    
         if(PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == false){ 

           GameObject player1 =   Instantiate(_playersObj[0],_spawnPoints[0].transform.position,_spawnPoints[0].transform.rotation);
            GameObject player2 =  Instantiate(_playersObj[1],_spawnPoints[1].transform.position,_spawnPoints[1].transform.rotation);
            _CurrentPlayers.Add(player1);
            _CurrentPlayers.Add(player2);
         
             SwitchControll();
        }else{

          foreach(GameObject obj in offlineModeobj ){
            obj.SetActive(false);

          }

        }


    }

    
    
    public void SwitchControll(){

    /*     _CurrentPlayers[0].GetComponent<PlayerController>().enabled = true;
          _CurrentPlayers[1].GetComponent<PlayerController>().enabled = true; */
         // _CurrentPlayers[1].GetComponent<PhotonView>().ControllerActorNr = false;
        _CurrentPlayers[0].GetComponent<PlayerControllOFFlineMode>()._mycontroll = 0;
          _CurrentPlayers[1].GetComponent<PlayerControllOFFlineMode>()._mycontroll = 1; 
        _CurrentPlayers[0].GetComponent<PlayerControllOFFlineMode>().myleader = _scoreBoard[0];
          _CurrentPlayers[1].GetComponent<PlayerControllOFFlineMode>().myleader = _scoreBoard[1];
             _CurrentPlayers[0].GetComponent<PlayerControllOFFlineMode>()._camera = _Cameras[1];
               _CurrentPlayers[1].GetComponent<PlayerControllOFFlineMode>()._camera = _Cameras[0]; 
    }

  
}
