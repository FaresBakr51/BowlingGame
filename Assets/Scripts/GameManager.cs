using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class GameManager : MonoBehaviourPunCallbacks
{

    private GameObject _leaderBoardobj;
    private PhotonView _photonview;

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
    
    private void Start(){
       
        _photonview = GetComponent<PhotonView>();

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
            if(PhotonNetwork.OfflineMode == false){


            
           CreatPlayer();
            }
        }

    }
     private void CreatPlayer(){
           PhotonNetwork.Instantiate("PhotonNetworkAvatar",transform.position,transform.rotation, 0);
    } 

}

