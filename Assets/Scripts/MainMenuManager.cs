using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviourPunCallbacks
{
  
   

 
    public Button _playbutt;
    public Button _compettbutt;
    public GameObject _roomsPanel;
    
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _leaderBoardPanel;
    public GameObject[] _mainMenubuttns;
    
    public GameObject _PickPlayerPanel;

    private bool _offlinemode;
    public GameObject[] _guidePic;
    public GameObject _guidPanel;

    public GameObject[] _CharacterButtons;
   
    [SerializeField] private GameObject _WAITINPanel;
    public static int _totalRankedPoints;

    [SerializeField] private Text _rankedpointTxt;
    
    public void ActiveRoompanel(){

       if(!PhotonNetwork.IsConnected){
         _mainPanel.SetActive(true);
         
        SetSelectedGameObject(_mainMenubuttns[0]);
       }else{
        _offlinemode = false;
      _PickPlayerPanel.SetActive(true);
       _mainPanel.SetActive(false);
       }
      
    }
 
    
   
    public void ShowCredits(){

      SceneManager.LoadScene(1);
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
    public void Back() {


        if (_roomsPanel.activeInHierarchy) {
            _roomsPanel.SetActive(false);
        }
        if (_PickPlayerPanel.activeInHierarchy) {

            _PickPlayerPanel.SetActive(false);
        }
        if (_leaderBoardPanel.activeInHierarchy) {

            _leaderBoardPanel.SetActive(false);
        }
        if (GameModes._rankedMode)
        {
            GameModes._rankedMode = false;
        }
        if (_WAITINPanel.activeInHierarchy)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            GameModes._rankedMode = false;
            StartCoroutine(Leavroom());
            _WAITINPanel.SetActive(false);
        }
          
        SetSelectedGameObject(_mainMenubuttns[0]);
        _mainPanel.SetActive(true);
    }
    IEnumerator Leavroom()
    {

        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
       
        PhotonNetwork.Reconnect();

    }

        void Start()
    {
        AudioListener.volume = 1;
        GameModes._rankedMode = false;
        GameModes._battleRoyale = false;
        StartCoroutine(GetRankedPoints());
       PhotonNetwork.OfflineMode = false;
       _mainMenubuttns[0].GetComponentInChildren<Image>().enabled =true;
         PhotonNetwork.ConnectUsingSettings();
        _offlinemode = false;
      _mainPanel.SetActive(true);
      SetSelectedGameObject(_mainMenubuttns[0]);
      
    }
   
    IEnumerator GetRankedPoints()
    {
        yield return new WaitForSeconds(3f);
        _totalRankedPoints = PlayerPrefs.GetInt("rankedpoints", 0);
        _rankedpointTxt.text = PhotonNetwork.LocalPlayer.NickName + " / " + _totalRankedPoints.ToString();
    }
    public void CreatRankedMatch()
    {
        GameModes._rankedMode = true;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
       
        CreatRoom();
    }
    private void CreatRoom()
    {
       
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
      
    }
  
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (GameModes._rankedMode)
        {
          
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(Random.Range(2, 4));
            }
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
 
   public void SelectCharacter(string ch){

       PlayerPrefs.SetString("character",ch);
       foreach(GameObject obj in _CharacterButtons){
         obj.GetComponent<Button>().interactable = true;
       }
        PlayerPrefs.Save();
        SetSelectedGameObject(_mainMenubuttns[0]);
        CheckGameMode();
       
   }
   public void playersmode(){

     PhotonNetwork.Disconnect();
     StartCoroutine(Join2PMODE());
   }
   private void CheckGameMode(){

     _PickPlayerPanel.SetActive(false);
        if (!GameModes._rankedMode)
        {
            if (_offlinemode == true)
            {

                PhotonNetwork.Disconnect();
                StartCoroutine(DisconnectJoinPractice());
            }
            else
            {

                if (!PhotonNetwork.InLobby || !PhotonNetwork.IsConnected)
                {

                    _mainPanel.SetActive(true);
                    SetSelectedGameObject(_mainMenubuttns[0]);
                };

                SetSelectedGameObject(_mainMenubuttns[2]);
                _roomsPanel.SetActive(true);
                _mainPanel.SetActive(false);
            }
        }
        else
        {
            if (!PhotonNetwork.InLobby || PhotonNetwork.InRoom)
            {
                _mainPanel.SetActive(true);
                SetSelectedGameObject(_mainMenubuttns[0]);
            }
            else
            {
                _WAITINPanel.SetActive(true);
                SetSelectedGameObject(_mainMenubuttns[11]);
                PhotonNetwork.JoinRandomRoom(null, 2);
            }
     
           
        }
   }
      public void JoinRoom()
    {
   
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
            PhotonNetwork.LoadLevel(Random.Range(2, 4));
        } 
    }
     IEnumerator Join2PMODE()
    {
        yield return new WaitForSeconds(1f);
      PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.LoadLevel(Random.Range(2, 4));


    }

}
