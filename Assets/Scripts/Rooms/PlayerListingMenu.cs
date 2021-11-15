using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerListing _playerlisting;
    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsController _controller;
    [SerializeField] private Button _startButt;
    [SerializeField] private GameObject _currentroompanel;

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

        int index = _listings.FindIndex(x => x.Player == player);
        if(index != -1){
            _listings[index].SetPlayerInfo(player);
        }else{
            PlayerListing listing = Instantiate(_playerlisting,_content);
           if(listing !=null){
               listing.SetPlayerInfo(player);
               _listings.Add(listing);
           }
        }
         if(!PhotonNetwork.IsMasterClient){
             _startButt.interactable = false;
             
         }
           

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       
         AddPlayerListing(newPlayer);
      
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      int index = _listings.FindIndex(x => x.Player == otherPlayer);
               if(index != -1){

                   Destroy(_listings[index].gameObject);
                   _listings.RemoveAt(index);
               }


                 if(!PhotonNetwork.IsMasterClient){
             _startButt.interactable = false;
             
         }else{
                _startButt.interactable = true;
         }

                
    }
    void Update(){

         if(SceneManager.GetActiveScene().name == "MainMenu" && _currentroompanel.activeInHierarchy == true){
               if (Input.GetButtonDown("trianglebutton"))
            {
                if(PhotonNetwork.InRoom){
                
                  Onclick_StartGame();
                }
            
            }
        }
    }
    public void Onclick_StartGame(){
        if(PhotonNetwork.IsMasterClient){
            if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers || PhotonNetwork.CurrentRoom.PlayerCount >=2){
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
            }
        }else{

        }
    }
  
}
