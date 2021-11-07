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
    private GameObject[] _spawnPoints;
    private PhotonView _photonview;
  //  public bool _competitive;
    public Button _playbutt;
    public Button _compettbutt;
      public GameObject _roomsPanel;
      private int PlayerIngame;
      private RoomOptions _roomoption;
      private int _rand;
    public static GameManager instance;
    
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
   


    }
    
    public void ActiveRoompanel(){
        _roomsPanel.SetActive(true);
    }
    private void Start(){
       
       _playbutt.enabled = false;
        _compettbutt.enabled = false;
        _photonview = GetComponent<PhotonView>();
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
      //   _competitive = false;
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        _playbutt.enabled = true;
        _compettbutt.enabled = true;
        Debug.Log("inlobby");
    }
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneLoadingFinished;

    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneLoadingFinished;
    }
    
    private void OnSceneLoadingFinished(Scene lvl, LoadSceneMode mode)
    {

        if (lvl.name == "Map1")
        {
       _spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
         CreatPlayer();
        }

    }
 
    private void CreatPlayer(){
     //if(_competitive == false){
    
     if(PlayerIngame == PhotonNetwork.CurrentRoom.PlayerCount){
       PhotonNetwork.Instantiate("Player", _spawnPoints[PlayerIngame].transform.position, _spawnPoints[PlayerIngame].transform.rotation, 0);
     }
     }
   // }



    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreatRoom();
    }
  
    private void CreatRoom()
    {
      
         _roomoption = new RoomOptions()
        {
           
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 4

        };
        
        
        
        
      
        PhotonNetwork.CreateRoom(null, _roomoption);
    
    }
    public override void OnJoinedRoom()
    {
        if(_roomsPanel.activeInHierarchy == false){
       PlayerIngame += 1;
        HandleNormalRoom();
        }
    }

    private void HandleNormalRoom(){
      
         PhotonNetwork.LoadLevel(1);
         Debug.Log("no competitive");
    }
    



   
    
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }

    }

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
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Input.GetButtonDown("square"))
            {
                JoinRoom();
            }
        }


        //  PhotonNetwork.Reconnect();
    }
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

