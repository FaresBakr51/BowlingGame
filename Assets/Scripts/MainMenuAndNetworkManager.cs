using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenuAndNetworkManager : MonoBehaviourPunCallbacks
{
  
   

 
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
    public GameObject[] _guidePic;
    public GameObject _guidPanel;
    [SerializeField] private GameObject[] _onlineSubmenu;
    public GameObject[] _CharacterButtons;

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

    public void ActiveSubMenu(string submenusName)
    {
        switch (submenusName)
        {
            case "online":
                
                for (int i = 0; i < _onlineSubmenu.Length; i++)
                {
                    if (_onlineSubmenu[i].activeInHierarchy)
                    {
                        _onlineSubmenu[i].SetActive(false);

                        for (int k = 2; k < mainButtons.Length; k++)
                        {
                            mainButtons[k].SetActive(true);
                            
                        }

                      
                    }
                    else
                    {
                        for (int k = 2; k < mainButtons.Length; k++)
                        {
                            mainButtons[k].SetActive(false);
                            
                        }

                        _onlineSubmenu[i].SetActive(true);
                    }
                }

                break;
            case "solo":
                break;
        }
       
        
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
       if(indx >= mainButtons.Length) return;
        mainButtons[indx].SetActive(true);
        indx++;
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
      UdpateSoundSource(_playerAudio, _uiclips[0]);
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
            case "Mrbill":
                indx = 1;
                break;
            case "Izzy":
                indx = 2;
                break;
            case "Barney":
                indx = 3;
                break;
            case "Cindy":
                indx = 4;
                break;
            case "Carl":
                indx = 5;
                break;
            case "Jong":
                indx = 6;
                break;
            case "Sergent Major":
                indx = 7;
                break;
        }

        return indx;
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

        if (GameModes._rankedMode)
        {
            
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("selectedcharacter", ch);
            localSelectedCh = ch;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
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
            if (_offlinemode)
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
    IEnumerator StartRankedMatch()
    {
        yield return new WaitForSeconds(2);
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.LoadLevel(Random.Range(2, 4));
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
