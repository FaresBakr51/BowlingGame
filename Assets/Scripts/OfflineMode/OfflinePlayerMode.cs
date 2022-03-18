using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class OfflinePlayerMode : MonoBehaviour
{
  [SerializeField] private Transform[] _spawnPoints;
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
 [SerializeField] private GameObject[] _SelectionButtonsPlayer1;
 [SerializeField] private GameObject[] _SelectionButtonsPlayer2;

    void Awake()
    {



      
         if(PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == false){ 
           _SelectorPane.SetActive(true);
               foreach(GameObject obj in _SelectionButtonsPlayer2){
        obj.SetActive(false);
      }
           EventSystem.current.SetSelectedGameObject(_player1Firstchar);
        }else{

          foreach(GameObject obj in offlineModeobj ){
            obj.SetActive(false);

          }

        }


    }

    public void SelectCharacter1(GameObject pl){
      player1 = pl;
      foreach(GameObject obj in _SelectionButtonsPlayer2){
        obj.SetActive(true);
      }
       foreach(GameObject obj in _SelectionButtonsPlayer1){
        obj.GetComponent<Button>().interactable = true;
      }
        EventSystem.current.SetSelectedGameObject(_player2Firstchar);
    }
    public void SelectCharacter2(GameObject pl2){
      player2 = pl2;
       foreach(GameObject obj in _SelectionButtonsPlayer2){
        obj.GetComponent<Button>().interactable = true;
      }
        EventSystem.current.SetSelectedGameObject(_startButt);
    }
    public void StartGame(){
      
     
      _SelectorPane.SetActive(false);
     
    GameObject playerNum1=   Instantiate(player1,_spawnPoints[0].transform.position,_spawnPoints[0].transform.rotation);
        GameObject playerNum2= Instantiate(player2,_spawnPoints[1].transform.position,_spawnPoints[1].transform.rotation);
         _CurrentPlayers.Add(playerNum1);
      _CurrentPlayers.Add(playerNum2);
       SwitchControll();
       
    }
    public void Back(){
      SceneManager.LoadScene(0);
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
