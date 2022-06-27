using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    private GameObject _player;
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
    private void Update()
    {
        if (!GameModes._arcadeMode) return;
         
     }
    private void OnSceneLoadingFinished(Scene lvl, LoadSceneMode mode)
    {

        if (lvl.name == "Map1" || lvl.name == "Map2" || lvl.name == "Map3" || lvl.name == "Map4")
        {
            if(!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode &&PhotonNetwork.InRoom )){
           CreatPlayer();
            }
          
        }

    }
    private void ControllArcadeMode()
    {
        if(_player.GetComponent<PlayerController>()._scoreplayer.totalscre > 0)
        {

        }
    }
     private void CreatPlayer(){
        GameObject avatar =    PhotonNetwork.Instantiate("PhotonNetworkAvatar",transform.position,transform.rotation, 0);
        _player = avatar;


    } 

  
   
  
}

