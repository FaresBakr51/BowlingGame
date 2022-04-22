using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviourPunCallbacks
{
  
    public RoomInfo RoomInfo {get;private set;}
    [SerializeField] private  TextMeshProUGUI _text;
    public bool _inGame;
    public void SetRoomInfo(RoomInfo roominfo){

        RoomInfo = roominfo;
        _text.text  =  roominfo.Name;
        
    }
    
  
      public void JoinRoomButt(){

          if(RoomInfo.IsOpen && PhotonNetwork.IsConnected){

            if (RoomInfo.MaxPlayers == 15)
            {
                GameModes._battleRoyale = true;
            }
        PhotonNetwork.JoinRoom(RoomInfo.Name);
          }
    }
 

}
