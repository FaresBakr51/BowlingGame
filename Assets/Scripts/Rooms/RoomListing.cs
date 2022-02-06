using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{

    public RoomInfo RoomInfo {get;private set;}
    [SerializeField] private  TextMeshProUGUI _text;
    public void SetRoomInfo(RoomInfo roominfo){

        RoomInfo = roominfo;
        _text.text  =  roominfo.Name;
    }
      public void JoinRoomButt(){

        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

}
