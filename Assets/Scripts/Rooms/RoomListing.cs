using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class RoomListing : MonoBehaviour
{

    public RoomInfo RoomInfo {get;private set;}
    [SerializeField] private  TextMeshProUGUI _text;
    //[SerializeField] private GameObject _currentRoomPanel;
    public void SetRoomInfo(RoomInfo roominfo){

        RoomInfo = roominfo;
        _text.text  =  roominfo.Name;
    }
      public void JoinRoomButt(){

        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
    void Update(){
         if(SceneManager.GetActiveScene().name == "MainMenu" ){
               if (Input.GetButtonDown("square"))
            {
                if(!PhotonNetwork.InRoom){
                
                  JoinRoomButt();
                }
            
            }
        }
    }
}
