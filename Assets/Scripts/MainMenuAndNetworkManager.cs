using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//using InfinityGameTable;
using System;
using UnityEngine.Events;

public class MainMenuAndNetworkManager : MonoBehaviourPunCallbacks
{

    [Header("PlatformEvents")]
    public static Action GetRankedPointsAction;

   // public static event Action SteamEvents;
  //  public static event Action IGTEvents;
 //   public static event Action PolyCadeBuild;
    
  //  private Dictionary<string, Action> platformActions = new Dictionary<string, Action>();
    [Header("Plaftorms")]
    public GameEventType gamePlatform;
    public string buildname;
    //public GameObject steamApply;
    [SerializeField]
  
 //   private readonly List<string> platformnames = new List<string>() { "steam","IGT","PolyCade" };
 //   private bool Initialized => platformTypes.Count > 0;

    public Button _playbutt;
    public Button _compettbutt;
    public GameObject _roomsPanel;
    
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _leaderBoardPanel;
    public GameObject[] _mainMenubuttns;


    [Header("MainButtonsActions")] 
    public int indx;
    public GameObject[] mainButtons;
    public GameObject _PickPlayerPanel;
    private bool _offlinemode;
   
    [SerializeField] private GameObject[] _submenuButtons;
    public GameObject[] _CharacterButtons;
    private bool _canActiveSub;
    [Header("RankedPanel")]
    [SerializeField] private GameObject _WAITINPanel;
    public static int _totalRankedPoints;
    [SerializeField] private Text _rankedpointTxt;
    public GameObject _vsPanel;
    private string localSelectedCh;
    public Sprite[] _bowlersSprites;
    public TextMeshProUGUI _mylocalname;
    public Image _mylocalImage;
    public TextMeshProUGUI _enemylocalname;
    public Image _enemylocalImage;
    
    [Header("Audio")]
    public AudioSource _playerAudio;
    
    public AudioClip[] _uiclips;

    [Header("ArcadePanel")]
    [SerializeField] private GameObject _arcadePanel;
    [SerializeField] private GameObject _massegepanel;
    [SerializeField] private Text _arcadegametxt;

    [Header("Achivement")]
   
    [SerializeField] private readonly static List<string> _achivmentCharacters = new List<string>() { "isaiah" };
    [SerializeField] private List<Button> _lockedButtons= new List<Button>();
    [SerializeField] private List<GameObject> _lockedImages = new List<GameObject>();

    public override void OnEnable()
    {
     
        GetRankedPointsAction += LoadRankedPoints;
    }
    public override void OnDisable()
    {
        GetRankedPointsAction -= LoadRankedPoints;
    }
    public void ActiveSubMenu(string submenusName)
    {
        if (!_canActiveSub) return;
        switch (submenusName)
        {
            case "online":
                
                for (int i = 2; i < _submenuButtons.Length; i++)
                {
                    if (_submenuButtons[i].activeInHierarchy)
                    {
                        _submenuButtons[i].SetActive(false);

                        for (int k = 0; k < mainButtons.Length; k++)
                        {
                            if (k != 2)
                            {
                                mainButtons[k].SetActive(true);
                            }
                            
                        }
                     
                      
                    }
                    else
                    {
                        for (int k = 0; k < mainButtons.Length; k++)
                        {
                            if (k != 2)
                            {
                                mainButtons[k].SetActive(false);
                            }
                        }

                        _submenuButtons[i].SetActive(true);
                        if (i == 2)
                        {
                            EventSystem.current.SetSelectedGameObject(_submenuButtons[i]);
                        }
                    }
                }

                break;
            case "solo":
                for (int i = 0; i < _submenuButtons.Length; i++)
                {
                    if (_submenuButtons[i].activeInHierarchy)
                    {
                        _submenuButtons[i].SetActive(false);

                        for (int k = 1; k < mainButtons.Length; k++)
                        {
                           
                             mainButtons[k].SetActive(true);
                            

                        }


                    }
                    else
                    {
                        
                        for (int k = 1; k < mainButtons.Length; k++)
                        {
                            mainButtons[k].SetActive(false);

                        }


                        _submenuButtons[i].SetActive(true);
                        if (i == 0)
                        {
                            EventSystem.current.SetSelectedGameObject(_submenuButtons[i]);
                        }
                    }
                }
                break;
        }
       
        
    }
    private void DisableSubMenusBacktoDefult()
    {
     
      
        foreach(GameObject obj in mainButtons) { if (!obj.activeInHierarchy) { obj.SetActive(true); } }
        foreach (GameObject obj in _submenuButtons) {{ obj.SetActive(false); } }
    }
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

    public void UdpateSoundSource(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.PlayOneShot(clip);
    }

    public void PlayNextMainButtAnimation()
    {
        if (indx >= mainButtons.Length - 1)
        {
            _canActiveSub = true;
        }
        else
        {
            mainButtons[indx].SetActive(true);
            indx++;
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
        if (_arcadePanel.activeInHierarchy)
        {
            _arcadePanel.SetActive(false);
        }

        if (GameModes._rankedMode)
        {
            GameModes._rankedMode = false;
        }
        if (GameModes._arcadeMode)
        {
            GameModes._arcadeMode = false;
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
        DisableSubMenusBacktoDefult();
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
        BuildPlatform(gamePlatform);
        AudioListener.volume = 1;
        GameModes._rankedMode = false;
        GameModes._arcadeMode = false;
        GameModes._battleRoyale = false;
        GameModes._2pMode = false;
      //  StartCoroutine(GetRankedPoints());
       PhotonNetwork.OfflineMode = false;
       _mainMenubuttns[0].GetComponentInChildren<Image>().enabled =true;
         PhotonNetwork.ConnectUsingSettings();
        _offlinemode = false;
     // _mainPanel.SetActive(true);
      SetSelectedGameObject(_mainMenubuttns[0]);
      UdpateSoundSource(_playerAudio, _uiclips[0]);
        RetriveData(_lockedButtons,_lockedImages);
    }

     private void LoadRankedPoints()
    {
        StartCoroutine(GetRankedPoints());
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
            newPlayer.CustomProperties.TryGetValue("selectedcharacter", out var value);
            photonView.RPC("RPCSendToMaster", RpcTarget.All, newPlayer.NickName,value.ToString());
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedcharacter", out var master);
            photonView.RPC("RPCSendtoClient", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName,master.ToString());
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
               
               photonView.RPC("RPCRunRankedGame",RpcTarget.All);
               if (PhotonNetwork.IsMasterClient)
               {
                   StartCoroutine(StartRankedMatch());
               }

            }
        }
    }

    [PunRPC]
    private void RPCRunRankedGame()
    {
        UdpateSoundSource(_playerAudio, _uiclips[1]);
        _vsPanel.SetActive(true);
        _mylocalname.text = PhotonNetwork.LocalPlayer.NickName;
        _mylocalImage.sprite = _bowlersSprites[GetPlayerSpriteInx(localSelectedCh)];
    }
    [PunRPC]
    private void RPCSendToMaster(string ch,string spriteindex )
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _enemylocalname.text = ch;
            _enemylocalImage.sprite = _bowlersSprites[GetPlayerSpriteInx(spriteindex)];
        }
        
    }
    [PunRPC]
    private void RPCSendtoClient(string ch,string spriteindex)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            _enemylocalname.text = ch;
            _enemylocalImage.sprite = _bowlersSprites[GetPlayerSpriteInx(spriteindex)];
        }
       
    }

    private int GetPlayerSpriteInx(string ch)
    {

        int indx = 0;
        switch (ch)
        {
            case "Paul":
                indx = 0;
                break;
            case "Barney":
                indx = 1;
                break;
            case "Carl":
                indx = 2;
                break;
            case "Cindy":
                indx = 3;
                break;
            case "Izzy":
                indx = 4;
                break;
            case "Jong":
                indx = 5;
                break;
            case "Sergent Major":
                indx = 6;
                break;
            case "Mrbill":
                indx = 7;
                break;
            case "isaiah":
                indx = 8;
                break;
        }

        return indx;
    }



    public override void OnConnectedToMaster()
    {
      if(!PhotonNetwork.InLobby && PhotonNetwork.OfflineMode == false){
        PhotonNetwork.JoinLobby();
      }
        Debug.Log("ConnectedToMaster");
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
        if (GameModes._arcadeMode)
        {
            PlayerPrefs.SetInt("selectedplayerindx", GetPlayerSpriteInx(ch));
            PlayerPrefs.SetString("arcadech", ch);
        }
        if (GameModes._rankedMode)
        {
            
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("selectedcharacter", ch);
            localSelectedCh = ch;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        PlayerPrefs.Save();
        CheckGameMode();
     
    }
    

    public void playersmode(){

 //    PhotonNetwork.Disconnect();
     StartCoroutine(Join2PMODE());
   }
   private void CheckGameMode(){

        if (_PickPlayerPanel.activeInHierarchy) { _PickPlayerPanel.SetActive(false); }
   
        if (!GameModes._rankedMode)
        {
            if (_offlinemode)
            {

       
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
                GameModes._rankedMode = false;
                _mainPanel.SetActive(true);
                SetSelectedGameObject(_mainMenubuttns[1]);
            }
            else
            {
              
                _WAITINPanel.SetActive(true);
                SetSelectedGameObject(_mainMenubuttns[7]);
                PhotonNetwork.JoinRandomRoom(null, 2);
            }
     
           
        }
   }
    public void CreatArcadeMatch()
    {
        GameModes._arcadeMode = true;
        if (PlayerPrefs.HasKey("ai"))
        {
            PlayerPrefs.DeleteKey("ai");
        }
        if (PlayerPrefs.HasKey("selectedai"))
        {
            PlayerPrefs.DeleteKey("selectedai");
        }
        if (PlayerPrefs.HasKey("selectedplayerindx"))
        {
            PlayerPrefs.DeleteKey("selectedplayerindx");
        }
        _offlinemode = true;
        _PickPlayerPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }
    public void ContinueArcadeMatch()
    {
        if (PlayerPrefs.HasKey("selectedai") && PlayerPrefs.HasKey("arcadech"))
        {
               _arcadegametxt.text = "NOW LOADING ...";
            GameModes._arcadeMode = true;
            _offlinemode = true;
            _mainPanel.SetActive(false);
            CheckGameMode();
        }
        else
        {

            _arcadegametxt.text = "NO DATA SAVED !";
        }
      
    }
      public void JoinRoom()
    {
   
      _offlinemode = true;
      _PickPlayerPanel.SetActive(true);
      _mainPanel.SetActive(false);
    }

    public static  void RetriveData(List<Button> lockedButtons, List<GameObject> lockedImages)
    {
       foreach(string s in _achivmentCharacters)
        {
            if (PlayerPrefs.HasKey(s))
            {

                lockedButtons[PlayerPrefs.GetInt(s)].enabled  =  true;
                lockedImages[PlayerPrefs.GetInt(s)].SetActive(false);
            }
        }
    }
    public static void UnlouchAchivment(string name,int characterid)
    {
        if (!PlayerPrefs.HasKey(name))
        {
            PlayerPrefs.SetInt(name, characterid);
        }
    
    }
  public void ExitApplication()
    {
        #region IGT
        
    //    InfinityGameTableHelper.QuitToDashboard();
        #endregion
        Application.Quit();

    }
    IEnumerator StartRankedMatch()
    {
        yield return new WaitForSeconds(2);
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.LoadLevel(UnityEngine.Random.Range(2, 4));
        }
   
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
            PhotonNetwork.LoadLevel(UnityEngine.Random.Range(2, 4));
        } 
    }
     IEnumerator Join2PMODE()
    {
        GameModes._2pMode = true;
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.LoadLevel(UnityEngine.Random.Range(2, 4));


    }

    #region PlatformsChanges

    
    private void BuildPlatform(GameEventType pname)
    {
        GameEventBus.Publish(pname);

    }
    #endregion

}
