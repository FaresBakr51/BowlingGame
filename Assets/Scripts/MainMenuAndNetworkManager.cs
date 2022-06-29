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
    private int indx;
    public GameObject[] mainButtons;
    public GameObject _PickPlayerPanel;

    private bool _offlinemode;
    public GameObject[] _guidePic;
    public GameObject _guidPanel;

    public GameObject[] _CharacterButtons;

    [Header("RankedPanel")]
    [SerializeField] private GameObject _WAITINPanel;
    public static int _totalRankedPoints;
    [SerializeField] private Text _rankedpointTxt;
    
    public TextMeshProUGUI _myCharName;
    public TextMeshProUGUI _EnemyCharName;
    public GameObject _waitTimeOBj;
    public TextMeshProUGUI _waitTime;
    public int waitTime;

 
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
            photonView.RPC("RPCSendToMaster", RpcTarget.All, value.ToString());
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedcharacter", out var master);
            photonView.RPC("RPCSendtoClient", RpcTarget.All, master.ToString());
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(StartRankedMatch());
              //  PhotonNetwork.LoadLevel(Random.Range(2, 4));
            }
        }
    }

    [PunRPC]
    private void RPCSendToMaster(string ch )
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _EnemyCharName.text = ch;
        }
        
    }
    [PunRPC]
    private void RPCSendtoClient(string ch)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            _EnemyCharName.text = ch;
        }
       
    }
    public override void OnJoinedRoom()
    {
        if (GameModes._rankedMode)
        {
            //if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            //{
            //    for (int i = 0; i <= PhotonNetwork.PlayerList.Length; i++)
            //    {

            //        if (PhotonNetwork.PlayerList[i].IsLocal && !PhotonNetwork.PlayerList[i].IsMasterClient)
            //        {
            //            var hash = PhotonNetwork.PlayerList[i - 1].CustomProperties.TryGetValue("selectedcharacter", out var hashValue);
            //            _EnemyCharName.text = hashValue.ToString();
            //        }
            //        if (PhotonNetwork.PlayerList[i].IsMasterClient)
            //        {
            //            Debug.Log("masterclient");
            //            var hash = PhotonNetwork.PlayerList[i+1].CustomProperties.TryGetValue("selectedcharacter", out var hashValue);
            //            _EnemyCharName.text = hashValue.ToString();
            //        }
            //    }
            //}
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
       

        //if(targetPlayer.IsMasterClient && targetPlayer.IsLocal)
        //{
        //    targetPlayer.CustomProperties.TryGetValue("selectedcharacter", out hasval1);
        //    if (hasval2 != null)
        //    {
        //        _EnemyCharName.text = hasval2.ToString();
        //    }
        
        //}
        //if (!targetPlayer.IsMasterClient && targetPlayer.IsLocal)
        //{
        //    targetPlayer.CustomProperties.TryGetValue("selectedcharacter", out hasval2);
        //    if (hasval1 != null)
        //    {
        //        _EnemyCharName.text = hasval1.ToString();
        //    }
        //}
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
             _myCharName.text = ch;
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
        _waitTimeOBj.SetActive(true);
        _waitTime.text = waitTime.ToString();
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel(Random.Range(2, 4));
        //if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        //{
        //    PhotonNetwork.LoadLevel(Random.Range(2, 4));
        //}
        //else
        //{
        //    _waitTimeOBj.SetActive(false);
        //    _EnemyCharName.text = "";
        //    //reset time player quit
        //    waitTime = 0;
        //}
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
