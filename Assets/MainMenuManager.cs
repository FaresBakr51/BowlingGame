using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 using UnityEngine.EventSystems;
 using UnityEngine.InputSystem;
public class MainMenuManager : MonoBehaviourPunCallbacks
{
       public Button _playbutt;
    public Button _compettbutt;
      public GameObject _roomsPanel;
      [SerializeField] private GameObject _roomsPanelPlayers;
       [SerializeField] private GameObject _mainPanel;
      public GameObject[] _mainMenubuttns;
        public GameObject[] _creatorjoinroompanelButtns;
      private GameObject _oldbtnselected;
      private GameObject _currentbtnselected;
      public GameObject _roomsContent;
       public Image _selectedPaul;
         public Image _selectedMrbill;
           public Image _selectpaul;
              public Image _selectmrbill;
     public void ActiveRoompanel(){
       // _practice = false;

    
   /*   if(_roomsContent.transform.GetChild(0) != null){

       SetSelectedGameObject(_roomsContent.transform.GetChild(0).gameObject);

     }else{ */

     if(!PhotonNetwork.InLobby)return;
        SetSelectedGameObject(_mainMenubuttns[2]);
    // }
        _roomsPanel.SetActive(true);
        _mainPanel.SetActive(false);
      
    }
    public void ActivealreadyRoompanel(){

      SetSelectedGameObject(_mainMenubuttns[2]);
    // }
        _roomsPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }
    public void ActiveCurrentRoompanel(){

      SetSelectedGameObject(_mainMenubuttns[4]);
      _roomsPanel.SetActive(false);
    }
    public void SetSelectedGameObject(GameObject selected){

     // EventSystem.current.SetSelectedGameObject(selected);
     selected.GetComponent<Button>().Select();
   

    }
    public void Back(){

        SetSelectedGameObject(_mainMenubuttns[0]);
        _roomsPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }


    void Start()
    {
       PhotonNetwork.OfflineMode = false;
     //  m_EventSystem =  EventSystem.current;
       _mainMenubuttns[0].GetComponentInChildren<Image>().enabled =true;
        //  _playbutt.enabled = false;
        //_compettbutt.enabled = false;
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

    public void TransferSelection(GameObject[] obj,int index){

      obj[index].transform.GetChild(1).GetComponent<Image>().enabled = true; 
      if(index > 0){
      obj[index-1].transform.GetChild(1).GetComponent<Image>().enabled = false; 
      }
     
    }
    // Update is called once per frame
   public void SelectPaul(){

     PlayerPrefs.SetInt("character",0);
    _selectpaul.enabled = false;
    _selectedPaul.enabled = true;
    _selectedMrbill.enabled = false;
    _selectmrbill.enabled = true;
    
   }
   public void SelectMrbill(){
       PlayerPrefs.SetInt("character",1);
    _selectpaul.enabled = true;
    _selectedPaul.enabled = false;
    _selectedMrbill.enabled = true;
    _selectmrbill.enabled = false;
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
