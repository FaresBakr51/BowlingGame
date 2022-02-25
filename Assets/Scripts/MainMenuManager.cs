using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviourPunCallbacks
{
       public Button _playbutt;
    public Button _compettbutt;
      public GameObject _roomsPanel;
      [SerializeField] private GameObject _roomsPanelPlayers;
       [SerializeField] private GameObject _mainPanel;
      public GameObject[] _mainMenubuttns;
    
              public GameObject _PickPlayerPanel;

              private bool _offlinemode;
              public GameObject[] _guidePic;
              public GameObject _guidPanel;
              public int _counter;

              public GameObject[] _CharacterButtons;
     public void ActiveRoompanel(){

       if(!PhotonNetwork.IsConnected)return;
        _offlinemode = false;
      _PickPlayerPanel.SetActive(true);
       _mainPanel.SetActive(false);
        SetSelectedGameObject(_mainMenubuttns[6]);
      
    }
    public void NextPic(){
     if(_counter != 3){
       _counter++;
      _guidePic[_counter].SetActive(true);
     }else{
       _guidPanel.SetActive(false);
        _mainPanel.SetActive(true);
        SetSelectedGameObject(_mainMenubuttns[0]);
     }
     

    }
    public void ShowTut(){

      _guidPanel.SetActive(true);
      _mainPanel.SetActive(false);
      SetSelectedGameObject(_mainMenubuttns[9]);
    }
    public void ShowCredits(){

      SceneManager.LoadScene(1);
    }
    public void Skip(){

      
       _guidPanel.SetActive(false);
        _mainPanel.SetActive(true);
        SetSelectedGameObject(_mainMenubuttns[0]);
    }
   
    public void ActivealreadyRoompanel(){

      SetSelectedGameObject(_mainMenubuttns[2]);
        _roomsPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }
    public void ActiveCurrentRoompanel(){

      SetSelectedGameObject(_mainMenubuttns[4]);
      _roomsPanel.SetActive(false);
    }
    public void SetSelectedGameObject(GameObject selected){
    selected.GetComponent<Button>().Select();
    }
    public void Back(){

        SetSelectedGameObject(_mainMenubuttns[0]);
        if(_roomsPanel.activeInHierarchy){
          _roomsPanel.SetActive(false);
        }
        if(_PickPlayerPanel.activeInHierarchy){

          _PickPlayerPanel.SetActive(false);
        }
        _mainPanel.SetActive(true);
    }


    void Start()
    {
       PhotonNetwork.OfflineMode = false;
       _mainMenubuttns[0].GetComponentInChildren<Image>().enabled =true;
         PhotonNetwork.ConnectUsingSettings();
           _offlinemode = false;
      _mainPanel.SetActive(true);
      SetSelectedGameObject(_mainMenubuttns[0]);
      
    }
 public override void OnConnectedToMaster()
    {
      if(!PhotonNetwork.InLobby && PhotonNetwork.OfflineMode == false){
        PhotonNetwork.JoinLobby();
      }
        PhotonNetwork.AutomaticallySyncScene = true;
        _playbutt.enabled = true;
        _compettbutt.enabled = true;
  
    }
 
   public void SelectCharacter(string ch){

       PlayerPrefs.SetString("character",ch);
       foreach(GameObject obj in _CharacterButtons){
         obj.GetComponent<Button>().interactable = true;
       }
       SetSelectedGameObject(_mainMenubuttns[0]);
        CheckGameMode();
   }
   public void playersmode(){

     PhotonNetwork.Disconnect();
     StartCoroutine(Join2PMODE());
   }
   private void CheckGameMode(){

     _PickPlayerPanel.SetActive(false);
     
     if(_offlinemode == true){

      PhotonNetwork.Disconnect();
     StartCoroutine(DisconnectJoinPractice());
     }else{
       
     if(!PhotonNetwork.InLobby)return;
     if(!PhotonNetwork.IsConnected)return;
        SetSelectedGameObject(_mainMenubuttns[2]);
        _roomsPanel.SetActive(true);
        _mainPanel.SetActive(false);
     }
   }
      public void JoinRoom()
    {
 SetSelectedGameObject(_mainMenubuttns[6]);
      _offlinemode = true;
      _PickPlayerPanel.SetActive(true);
      _mainPanel.SetActive(false);
    }
    IEnumerator DisconnectJoinPractice()
    {
       
      PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        PhotonNetwork.OfflineMode = true;
          if(PhotonNetwork.OfflineMode == true){
            PhotonNetwork.JoinRoom(null);
          PhotonNetwork.CurrentRoom.IsOpen = false;
           PhotonNetwork.CurrentRoom.IsVisible = false;
           // PhotonNetwork.LoadLevel(2);
            PhotonNetwork.LoadLevel(Random.Range(2,4));
      } 
    }
     IEnumerator Join2PMODE()
    {
       
      PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        PhotonNetwork.OfflineMode = true;
          //  PhotonNetwork.LoadLevel(2);
        PhotonNetwork.LoadLevel(Random.Range(2,4));
      
    }

 
   
  
    
}
