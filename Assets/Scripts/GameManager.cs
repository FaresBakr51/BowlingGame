using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{

    private GameObject _leaderBoardobj;
    private static GameManager _instance;
    private List<GameObject> _spawnPoints = new List<GameObject>();
    private PhotonView _photonview;
  //  public bool _competitive;
    public Button _playbutt;
    public Button _compettbutt;
      public GameObject _roomsPanel;
      [SerializeField] private GameObject _roomsPanelPlayers;
      private int PlayerIngame;
      private RoomOptions _roomoption;
      private int _rand;
    public static GameManager instance;
    //[SerializeField] private int _currentplayers;
    private int _currentplayers;
   // private bool _practice;
    public  int Currentplayers{

        get{
            return _currentplayers;
        }
        set{

            _currentplayers = value;
        }
    }
    private void Awake()
    {
      
        if (GameManager.instance == null)
        {
           
            GameManager.instance = this;
           
        }
        else 
        {
            if(GameManager.instance != this){
                Destroy(GameManager.instance.gameObject);
                GameManager.instance = this;
            }
        }
         DontDestroyOnLoad(this.gameObject);

       /*   _roomsPanel = GameObject.FindWithTag("creatorjoin");
         _roomsPanelPlayers = GameObject.FindWithTag("currentroom");
         _playbutt = GameObject.FindWithTag("practicebutt").GetComponent<Button>();
         _compettbutt = GameObject.FindWithTag("onlinebutt").GetComponent<Button>();
         _roomsPanelPlayers.SetActive(false);
         _roomsPanel.SetActive(false); */

    }
    
    /* public void ActiveRoompanel(){
       // _practice = false;
        _roomsPanel.SetActive(true);
    } */
   
    private void Start(){
       
       //_playbutt.enabled = false;
       // _compettbutt.enabled = false;
        _photonview = GetComponent<PhotonView>();
       // PhotonNetwork.ConnectUsingSettings();

    }

   /*  public override void OnConnectedToMaster()
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
 */    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneLoadingFinished;

    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneLoadingFinished;
    }
    
    private void OnSceneLoadingFinished(Scene lvl, LoadSceneMode mode)
    {

        if (lvl.name == "Map1")
        {
    //   _spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
       foreach(GameObject obj in GameObject.FindGameObjectsWithTag("spawnpoint") ){
           _spawnPoints.Add(obj);

           // CreatPlayer();
       }
        CreatPlayer();
        }

    }
     private void CreatPlayer(){

           if(PhotonNetwork.OfflineMode == false){
     Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    PhotonNetwork.Instantiate("PhotonNetworkAvatar",transform.position,transform.rotation, 0);
    _currentplayers ++;
 
    
     }else{
           PhotonNetwork.Instantiate("PhotonNetworkAvatar",transform.position,transform.rotation, 0);
     }
    } 
  /*   [PunRPC]
    private void CreatPlayerRpc(){

        _currentplayers++;
     } */
   // }



   /*  public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreatRoom();
    }
   */
   /*  private void CreatRoom()
    {
      
         _roomoption = new RoomOptions()
        {
           
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 4

        };
        
        
        
        
      
        PhotonNetwork.CreateRoom(null, _roomoption);
    
    } */
   /*  public override void OnJoinedRoom()
    {
        if(_roomsPanel.activeInHierarchy == false){
       PlayerIngame += 1;
        HandleNormalRoom();
        }
    } */

   /*  private void HandleNormalRoom(){
      
         PhotonNetwork.LoadLevel(1);
         Debug.Log("no competitive");
    }
     */



   
    
  /*   public void JoinRoom()
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
    } */
   
 

    public void RequestLeaderBoard()
    {
        if (_leaderBoardobj == null)
        {
            _leaderBoardobj = GameObject.FindWithTag("leaderboard");
        }
        StartCoroutine(Waitleader());
    }
    public void RequestFinalLeaderBoarD()
    {
        if (_leaderBoardobj == null)
        {
            _leaderBoardobj = GameObject.FindWithTag("leaderboard");
        }
        _leaderBoardobj.transform.GetChild(0).gameObject.SetActive(true);
    }
   /*  private void Update()
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
    } */
    IEnumerator Waitleader()
    {
        _leaderBoardobj.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        _leaderBoardobj.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void StartGame()
    {

    }
    private void GetSpawnPoints()
    {
      //  objs = GameObject.FindGameObjectsWithTag("spawnpoint");
    }
}

