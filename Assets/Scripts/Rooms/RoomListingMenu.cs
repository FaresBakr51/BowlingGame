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
    [SerializeField] private MainMenuAndNetworkManager _menuManager;
  //  [SerializeField] private List<RoomListing> _rankedMatches = new List<RoomListing>();
   
    public void FirstIniatlize(RoomsController _controll){

        _roomcontroll = _controll;
    }
    public override void OnJoinedRoom()
    {
        if(!PhotonNetwork.OfflineMode && !GameModes._rankedMode)
        {
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
            if (InGameRooms()!= -1 )
            {
                if (!_listings[InGameRooms()]._inGame)
                {
                    _listings[InGameRooms()].GetComponentInChildren<TextMeshProUGUI>().text = _listings[InGameRooms()].GetComponentInChildren<TextMeshProUGUI>().text + " (InGame)";
                    _listings[InGameRooms()].GetComponent<Image>().color = Color.red;
                    _listings[InGameRooms()]._inGame = true;
                }

            }
            if (AnyRankedRoom() != -1)
            {
                Destroy(_listings[AnyRankedRoom()].gameObject);
                _listings.RemoveAt(AnyRankedRoom());
            }
            if(AnyBattleRooms() != -1)
            {
                if (!_listings[AnyBattleRooms()]._inGame)
                {
                    _listings[AnyBattleRooms()].GetComponentInChildren<TextMeshProUGUI>().text = _listings[AnyBattleRooms()].GetComponentInChildren<TextMeshProUGUI>().text + " (RoyaleMode)";
                    _listings[AnyBattleRooms()].GetComponent<Image>().color = Color.green;
                    _listings[AnyBattleRooms()]._inGame = true;
                }

            }
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    
    {
        if (GameModes._rankedMode) return; 
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
                    if (!GameModes._rankedMode)
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
        int ingame = _listings.FindIndex(t => !t.RoomInfo.IsOpen && !t._inGame && t.RoomInfo.MaxPlayers != 15);
        return ingame;
    }
    int AnyBattleRooms()
    {
        int hasbattle = _listings.FindIndex(t => t.RoomInfo.MaxPlayers == 15);
        return hasbattle;
    }

}
