using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Realtime;
public class MainMenuManager : MonoBehaviourPunCallbacks
{
       public Button _playbutt;
    public Button _compettbutt;
      public GameObject _roomsPanel;
      [SerializeField] private GameObject _roomsPanelPlayers;
       [SerializeField] private GameObject _mainPanel;
      public GameObject[] _mainMenubuttns;
       public Image _selectedPaul;
         public Image _selectedMrbill;
           public Image _selectpaul;
              public Image _selectmrbill;
              public GameObject _PickPlayerPanel;

              private bool _offlinemode;
              public GameObject[] _guidePic;
              public GameObject _guidPanel;
              public int _counter;
     public void ActiveRoompanel(){
        _offlinemode = false;
      _PickPlayerPanel.SetActive(true);
       _mainPanel.SetActive(false);
        SetSelectedGameObject(_mainMenubuttns[6]);
      
    }
    public void NextPic(){
     if(_counter != 2){
       _counter++;
      _guidePic[_counter].SetActive(true);
     }
     

    }
    public void Skip(){

      PlayerPrefs.SetInt("guide",1);
       _guidPanel.SetActive(false);
        _mainPanel.SetActive(true);

       
        SetSelectedGameObject(_mainMenubuttns[0]);
    }
    void Update(){

     /*  if(_mainPanel.activeInHierarchy == true){

        if(EventSystem.current.currentSelectedGameObject == _mainMenubuttns[7]){

         StartCoroutine(SetDefultbutt());
        }
      }
 */
    }
    IEnumerator SetDefultbutt(){

      yield return new WaitForSeconds(3f);
       SetSelectedGameObject(_mainMenubuttns[8]);
      
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
       _mainMenubuttns[0].GetComponentInChildren<Image>().enabled =true;
         PhotonNetwork.ConnectUsingSettings();
           _offlinemode = false;
      _mainPanel.SetActive(true);
      SetSelectedGameObject(_mainMenubuttns[0]);
         if(PlayerPrefs.HasKey("guide")){
        _guidPanel.SetActive(false);
        _mainPanel.SetActive(true);
      }else{

        _mainPanel.SetActive(false);
        _guidPanel.SetActive(true);
        SetSelectedGameObject(_mainMenubuttns[9]);
      }
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

    public void TransferSelection(GameObject[] obj,int index){

      obj[index].transform.GetChild(1).GetComponent<Image>().enabled = true; 
      if(index > 0){
      obj[index-1].transform.GetChild(1).GetComponent<Image>().enabled = false; 
      }
     
    }
   public void SelectPaul(){

     PlayerPrefs.SetInt("character",0);
    _selectpaul.enabled = false;
    _selectedPaul.enabled = true;
    _selectedMrbill.enabled = false;
    _selectmrbill.enabled = true;
    SetSelectedGameObject(_mainMenubuttns[0]);
     CheckGameMode();
   }
   public void SelectMrbill(){
       PlayerPrefs.SetInt("character",1);
    _selectpaul.enabled = true;
    _selectedPaul.enabled = false;
    _selectedMrbill.enabled = true;
    _selectmrbill.enabled = false;
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
