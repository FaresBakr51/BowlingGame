using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class RoomListingMenu : MonoBehaviourPunCallbacks
{

    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomlisting;
    private List<RoomListing> _listings = new List<RoomListing>();

    private RoomsController _roomcontroll;
    [SerializeField] private Button _startbutt;
    [SerializeField] private MainMenuManager _menuManager;
    [SerializeField] private List<RoomListing> _rankedMatches = new List<RoomListing>();
   
    public void FirstIniatlize(RoomsController _controll){

        _roomcontroll = _controll;
    }
    public override void OnJoinedRoom()
    {
        if(!PhotonNetwork.OfflineMode && !GameManager.instance._rankedMode){
         _roomcontroll.CurrentRoomCanavas.Show();
         _content.DestroyChildren();
         _listings.Clear();
           _menuManager.ActiveCurrentRoompanel();
            if(!PhotonNetwork.IsMasterClient){
             _startbutt.gameObject.SetActive(false);
             EventSystem.current.SetSelectedGameObject(_menuManager._mainMenubuttns[4]);
             
         }
        }
    }
   void Update(){
        TypeRooms();
    }
   private void TypeRooms()
    {
        if (_listings.Count > 0)
        {
            if (InGameRooms()!= -1)
            {
             
                _listings[InGameRooms()].GetComponentInChildren<TextMeshProUGUI>().text = _listings[InGameRooms()].GetComponentInChildren<TextMeshProUGUI>().text + " (InGame)";
                _listings[InGameRooms()]._inGame = true;

            }
            if (AnyRankedRoom() != -1)
            {
                Destroy(_listings[AnyRankedRoom()].gameObject);
                _listings.RemoveAt(AnyRankedRoom());
            }
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    
    {
        if (GameManager.instance._rankedMode) return; 
        foreach (RoomInfo info in roomList){
           
         

           if(info.RemovedFromList){
               int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
               if(index != -1){

                   Destroy(_listings[index].gameObject);
                   _listings.RemoveAt(index);
               }

           }else{

                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
               if(index == -1){
                    if (!GameManager.instance._rankedMode)
                    {

                        RoomListing listing = Instantiate(_roomlisting, _content);                    
                            if (listing != null)
                            {
                                listing.SetRoomInfo(info);
                                _listings.Add(listing);
                            }
                        }
                    }
               }
           } 

       
      
    }
    int AnyRankedRoom()
    {

        int hasranked = _listings.FindIndex(t => t.RoomInfo.MaxPlayers == 2);
        return hasranked;

    }
    int InGameRooms()
    {
        int ingame = _listings.FindIndex(t => !t.RoomInfo.IsOpen && !t._inGame);
        return ingame;
    }
    
}
