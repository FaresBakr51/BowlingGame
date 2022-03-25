using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
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
    [SerializeField] private GameObject[] _soundOnOF;
     public GameObject _pauseMenupanel;
    public GameObject pausefirstbutt;
    public bool _gamePaused;
    public InputSystemUIInputModule[] _mine;
    public EventSystem[] _events;
    void Awake()
    {

      


        if (PhotonNetwork.OfflineMode == true && PhotonNetwork.InRoom == false){ 
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
       
        _mine[0].enabled = false;
        _events[0].enabled = false;
        _events[1].enabled = true;
        _mine[1].enabled = true;
        player2 = pl;
        foreach (GameObject obj in _SelectionButtonsPlayer2){
        obj.SetActive(true);
      }
       foreach(GameObject obj in _SelectionButtonsPlayer1){
        obj.GetComponent<Button>().interactable = false;
      }
        EventSystem.current.SetSelectedGameObject(_player2Firstchar);
    }
    public void SelectCharacter2(GameObject pl2){
       
     player1 = pl2;
       foreach(GameObject obj in _SelectionButtonsPlayer2){
        obj.GetComponent<Button>().interactable = false;
      }
        StartGame();
    }
    public void StartGame(){

       
      _SelectorPane.SetActive(false);
     
    GameObject playerNum1=   Instantiate(player1,_spawnPoints[0].transform.position,_spawnPoints[0].transform.rotation);
        GameObject playerNum2= Instantiate(player2,_spawnPoints[1].transform.position,_spawnPoints[1].transform.rotation);
         _CurrentPlayers.Add(playerNum1);
      _CurrentPlayers.Add(playerNum2);
       SwitchControll();
        _mine[0].enabled = true;
        _events[0].enabled = true;
       
        _mine[1].enabled = false;
        _events[1].enabled = false;
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
    public void Resume()
    {

        _pauseMenupanel.SetActive(false);
        StartCoroutine(WaitPause());
    }
    IEnumerator WaitPause()
    {
        yield return new WaitForSeconds(1);
        _gamePaused = false;
    }
    public void QuitGame()
    {

        SceneManager.LoadScene(0);


    }
    public void SoundOn()
    {

        AudioListener.volume = 1;
        EventSystem.current.SetSelectedGameObject(_soundOnOF[1]);

    }
    public void Soundoff()
    {

        AudioListener.volume = 0;
        EventSystem.current.SetSelectedGameObject(_soundOnOF[0]);
    }

}
