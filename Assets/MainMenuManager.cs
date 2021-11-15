using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviourPunCallbacks
{
       public Button _playbutt;
    public Button _compettbutt;
      public GameObject _roomsPanel;
      [SerializeField] private GameObject _roomsPanelPlayers;
   
   public void ActiveRoompanel(){
       // _practice = false;
        _roomsPanel.SetActive(true);
    }
    void Start()
    {
       PhotonNetwork.OfflineMode = false;
          _playbutt.enabled = false;
        _compettbutt.enabled = false;
         PhotonNetwork.ConnectUsingSettings();
    }
 public override void OnConnectedToMaster()
    {
      //   _competitive = false;

      if(!PhotonNetwork.InLobby && PhotonNetwork.OfflineMode == false){
        PhotonNetwork.JoinLobby();
      }
        PhotonNetwork.AutomaticallySyncScene = true;
        _playbutt.enabled = true;
        _compettbutt.enabled = true;
      
        Debug.Log("inlobby");
    }

    // Update is called once per frame
     private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" && _roomsPanel.activeInHierarchy == false && _roomsPanelPlayers.activeInHierarchy == false)
        {
           // _practice = true;
            if (Input.GetButtonDown("square"))
            {
                JoinRoom();
            }
        }

        if(SceneManager.GetActiveScene().name == "MainMenu"){
               if (Input.GetButtonDown("obutton"))
            {
                if(_roomsPanel.activeInHierarchy == false){
                ActiveRoompanel();
                }
            }
        }
          if(SceneManager.GetActiveScene().name == "MainMenu"){
               if (Input.GetButtonDown("xbutton"))
            {
                if(_roomsPanel.activeInHierarchy == true){
                 _roomsPanel.SetActive(false);
                }
            }
        }
      


        //  PhotonNetwork.Reconnect();
    }
      public void JoinRoom()
    {

      PhotonNetwork.Disconnect();
     StartCoroutine(GoOffline());
     

    }
    IEnumerator GoOffline(){
        yield return new WaitForSeconds(1f);
        PhotonNetwork.OfflineMode = true;
          if(PhotonNetwork.OfflineMode == true){
        // PhotonNetwork.CreateRoom(null);
         PhotonNetwork.JoinRoom(null);
         PhotonNetwork.CurrentRoom.IsOpen = false;
           PhotonNetwork.CurrentRoom.IsVisible = false;
         PhotonNetwork.LoadLevel(1);
      }
    }
}
