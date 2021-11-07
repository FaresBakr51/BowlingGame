using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{

    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomlisting;
    private List<RoomListing> _listings = new List<RoomListing>();

    private RoomsController _roomcontroll;
    public void FirstIniatlize(RoomsController _controll){

        _roomcontroll = _controll;
    }
    public override void OnJoinedRoom()
    {
         _roomcontroll.CurrentRoomCanavas.Show();
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
           RoomListing listing = Instantiate(_roomlisting,_content);
           if(listing !=null){
               listing.SetRoomInfo(info);
               _listings.Add(listing);
           }
           }
       }
    }
}
