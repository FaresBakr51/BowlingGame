using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
public class GameManager : MonoBehaviourPunCallbacks
{

    private GameObject _leaderBoardobj;
   

    public static GameManager instance;
    public bool _rankedMode;
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


    //private void Finieshed()
    //{
    //    if (_Players.Count >= 2)
    //    {
    //        FindWinner(_playerscores[0], _playerscores[1]);
    //    }
    //}
    //public void FindWinner(int p1,int p2)
    //{
    //    if (photonView.IsMine)
    //    {
    //        if (p1 > p2)
    //        {

    //            _Players[0].GetComponent<PlayerController>().ShowRankedResult("win");
    //            _Players[1].GetComponent<PlayerController>().ShowRankedResult("lose");

    //        }
    //        else if (p2 > p1)
    //        {
    //            _Players[0].GetComponent<PlayerController>().ShowRankedResult("lose");
    //            _Players[1].GetComponent<PlayerController>().ShowRankedResult("win");
    //        }
    //        else if (p2 == p1)
    //        {
    //            _Players[0].GetComponent<PlayerController>().ShowRankedResult("draw");
    //            _Players[1].GetComponent<PlayerController>().ShowRankedResult("draw");

    //        }

    //    }
    //}
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
            if(!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode &&PhotonNetwork.InRoom )){
           CreatPlayer();
            }
          
        }

    }
     private void CreatPlayer(){
        GameObject avatar =    PhotonNetwork.Instantiate("PhotonNetworkAvatar",transform.position,transform.rotation, 0);
       
    } 

  
   
  
}

