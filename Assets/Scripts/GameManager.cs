using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{

    private GameObject _leaderBoardobj;
   

    public static GameManager instance;
    public bool _rankedMode;
    [SerializeField] public int _finshedPlayers;
    [SerializeField] private List<GameObject> _Players = new List<GameObject>();
    [SerializeField] private List<int> _playerscores = new List<int>();
    private bool _rankedGameEnd;
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

    private void Update()
    {
        if (_rankedMode)
        {
            if (_finshedPlayers >=2 && !_rankedGameEnd)
            {
                Finieshed();
                _rankedGameEnd = true;
            }
           
    
        }
        
    }
    private void Finieshed()
    {
        if (_Players.Count >= 2)
        {
            FindWinner(_Players[0].GetComponent<PlayerController>()._scoreplayer.totalscre, _Players[1].GetComponent<PlayerController>()._scoreplayer.totalscre);
        }
    }
    public void FindWinner(int p1,int p2)
    {
        if(p1 > p2)
        {

            _Players[0].GetComponent<PlayerController>().ShowRankedResult("win");
            _Players[1].GetComponent<PlayerController>().ShowRankedResult("lose");

        }
        else if(p2 > p1)
        {
            _Players[0].GetComponent<PlayerController>().ShowRankedResult("lose");
            _Players[1].GetComponent<PlayerController>().ShowRankedResult("win");
        }
        else if(p2 == p1)
        {
            _Players[0].GetComponent<PlayerController>().ShowRankedResult("draw");
            _Players[1].GetComponent<PlayerController>().ShowRankedResult("draw");

        }
        

    }
    public override void OnEnable()
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

        if (lvl.name == "Map1" || lvl.name == "Map2" || lvl.name == "Map3")
        {
            if(PhotonNetwork.OfflineMode == false || (PhotonNetwork.OfflineMode == true &&PhotonNetwork.InRoom == true)){


            
           CreatPlayer();
            }
            StartCoroutine(WaitAvatars());
        }

    }
     private void CreatPlayer(){
        GameObject avatar =    PhotonNetwork.Instantiate("PhotonNetworkAvatar",transform.position,transform.rotation, 0);
       
    } 

    IEnumerator WaitAvatars()
    {

        yield return new WaitForSeconds(2f);
        if (_rankedMode)
        {

          
           foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                _Players.Add(player);
            }
        }
    }
   
  
}

