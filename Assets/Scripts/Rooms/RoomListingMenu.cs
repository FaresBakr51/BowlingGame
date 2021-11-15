using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class RoomListingMenu : MonoBehaviourPunCallbacks
{

    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomlisting;
    private List<RoomListing> _listings = new List<RoomListing>();

    private RoomsController _roomcontroll;
    [SerializeField] private Button _startbutt;
    public void FirstIniatlize(RoomsController _controll){

        _roomcontroll = _controll;
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.OfflineMode == false){
         _roomcontroll.CurrentRoomCanavas.Show();
         _content.DestroyChildren();
         _listings.Clear();
            if(!PhotonNetwork.IsMasterClient){
             _startbutt.interactable = false;
             
         }
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       foreach(RoomInfo info in roomList){
           if(info.RemovedFromList){
               int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
               if(index != -1){

                   Destroy(_listings[index].gameObject);
                   _listings.RemoveAt(index);
               }

           }else{
               Debug.Log("room instaniated");

          int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
          if(index == -1){
           RoomListing listing = Instantiate(_roomlisting,_content);
           if(listing !=null){
               listing.SetRoomInfo(info);
               _listings.Add(listing);
           }
           }else{

               //modify listing
           }
           }
       }
    }
}
