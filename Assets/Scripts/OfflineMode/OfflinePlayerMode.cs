using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class OfflinePlayerMode : MonoBehaviour
{
  [SerializeField] private Transform[] _spawnPoints;
  [SerializeField] private GameObject[] _playersObj;
   [SerializeField] private GameObject[] _Cameras;
    [SerializeField] private GameObject[] offlineModeobj;
 public GameObject[] _scoreBoard;
  public List<GameObject> _controllcanavas = new List<GameObject>();
 public List<GameObject> _CurrentPlayers = new List<GameObject>();
 public GameObject player1,player2;
 public GameObject _player1Firstchar;
 public GameObject _player2Firstchar;
 public GameObject _SelectorPane;
 public GameObject _startButt;
 [SerializeField] private GameObject[] _player2Selector;
 [SerializeField] private GameObject[] _SelectionButtonsPlayer1;
 [SerializeField] private GameObject[] _SelectionButtonsPlayer2;

    void Awake()
    {
      PhotonNetwork.OfflineMode = true;
      /* if(PhotonNetwork.OfflineMode == true &&PhotonNetwork.InRoom == true){

          PhotonNetwork.Instantiate("PhotonNetworkAvatar",transform.position,transform.rotation, 0);
      } */
    
         if(PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == false){ 
           _SelectorPane.SetActive(true);
               foreach(GameObject obj in _player2Selector){
        obj.SetActive(false);
      }
           EventSystem.current.SetSelectedGameObject(_player1Firstchar);
       
         /*   GameObject player1 =   Instantiate(_playersObj[0],_spawnPoints[0].transform.position,_spawnPoints[0].transform.rotation);
            GameObject player2 =  Instantiate(_playersObj[1],_spawnPoints[1].transform.position,_spawnPoints[1].transform.rotation);
            _CurrentPlayers.Add(player1);
            _CurrentPlayers.Add(player2); */
         
             //SwitchControll();
        }else{

          foreach(GameObject obj in offlineModeobj ){
            obj.SetActive(false);

          }

        }


    }

    public void SelectCharacter1(string pl){

      switch(pl){

        case "paul":
        player1 = _playersObj[0];
        break;
        case "izzy":
           player1 = _playersObj[2];
        break;
        case "mrbill":
           player1 = _playersObj[1];
        break;
      }
      foreach(GameObject obj in _player2Selector){
        obj.SetActive(true);
      }
       foreach(GameObject obj in _SelectionButtonsPlayer1){
        obj.GetComponent<Button>().interactable = true;
      }
        EventSystem.current.SetSelectedGameObject(_player2Firstchar);
    }
    public void SelectCharacter2(string pl){

      switch(pl){

        case "paul":
        player2 = _playersObj[0];
        break;
        case "izzy":
           player2 = _playersObj[2];
        break;
        case "mrbill":
           player2 = _playersObj[1];
        break;
      }
       foreach(GameObject obj in _SelectionButtonsPlayer2){
        obj.GetComponent<Button>().interactable = true;
      }
        EventSystem.current.SetSelectedGameObject(_startButt);
    }
    public void StartGame(){
      
      _CurrentPlayers.Add(player1);
      _CurrentPlayers.Add(player2);
      _SelectorPane.SetActive(false);
      SwitchControll();
       Instantiate(player1,_spawnPoints[0].transform.position,_spawnPoints[0].transform.rotation);
       Instantiate(player2,_spawnPoints[1].transform.position,_spawnPoints[1].transform.rotation);
    }
    public void Back(){
      SceneManager.LoadScene(1);
    }
    public void SwitchControll(){
        _CurrentPlayers[0].GetComponent<PlayerControllOFFlineMode>()._mycontroll = 0;
          _CurrentPlayers[1].GetComponent<PlayerControllOFFlineMode>()._mycontroll = 1; 
        _CurrentPlayers[0].GetComponent<PlayerControllOFFlineMode>().myleader = _scoreBoard[0];
          _CurrentPlayers[1].GetComponent<PlayerControllOFFlineMode>().myleader = _scoreBoard[1];
             _CurrentPlayers[0].GetComponent<PlayerControllOFFlineMode>()._camera = _Cameras[1];
               _CurrentPlayers[1].GetComponent<PlayerControllOFFlineMode>()._camera = _Cameras[0]; 
    }

  
}
