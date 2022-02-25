using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerListing _playerlisting;
    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsController _controller;
    [SerializeField] private GameObject _startButt;
    [SerializeField] private GameObject _currentroompanel;
    [SerializeField] private GameObject _WaitingPlayerMass;
    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayer();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        for(int i =0;i<_listings.Count; i++){

            Destroy(_listings[i].gameObject);
        }
        _listings.Clear();
    }
    public void FirstIniatlize(RoomsController controll){
        _controller = controll;

    }
  
    private void GetCurrentRoomPlayer(){
        if(!PhotonNetwork.IsConnected)
        return;
        if(PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
        return;
        foreach(KeyValuePair<int,Player> playerinfo in PhotonNetwork.CurrentRoom.Players){

             AddPlayerListing(playerinfo.Value);
        }
        
    }

    private void AddPlayerListing(Player player){

       /*  int index = _listings.FindIndex(x => x.Player == player);
        if(index != -1){
            _listings[index].SetPlayerInfo(player);
        }else{ */
            PlayerListing listing = Instantiate(_playerlisting,_content);
           if(listing !=null){
               listing.SetPlayerInfo(player);
               _listings.Add(listing);
           }
       // }
         if(!PhotonNetwork.IsMasterClient){
             _WaitingPlayerMass.SetActive(true);
             _startButt.SetActive(false);
        
         }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       
         AddPlayerListing(newPlayer);
         if(PhotonNetwork.IsMasterClient){
         if(PhotonNetwork.CurrentRoom.PlayerCount >=2){
             _WaitingPlayerMass.SetActive(false);
             _startButt.SetActive(true);
         }
         if(PhotonNetwork.CurrentRoom.PlayerCount <2){
                 _WaitingPlayerMass.SetActive(true);
             _startButt.SetActive(false);
         }
         }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      int index = _listings.FindIndex(x => x.Player == otherPlayer);
               if(index != -1){

                   Destroy(_listings[index].gameObject);
                   _listings.RemoveAt(index);
               }
                
    }
    public void Onclick_StartGame(){
        if(PhotonNetwork.IsMasterClient){
            if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers || PhotonNetwork.CurrentRoom.PlayerCount >=2){
            PhotonNetwork.CurrentRoom.IsOpen = false;
           // PhotonNetwork.CurrentRoom.IsVisible = false;
         var roomName = PhotonNetwork.CurrentRoom.Name;
            roomName = PhotonNetwork.CurrentRoom.Name + ": (InGame)";
            PhotonNetwork.LoadLevel(Random.Range(2,4));
            }
        }
    }
  
}
