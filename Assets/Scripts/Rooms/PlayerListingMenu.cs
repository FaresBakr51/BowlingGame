using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerListing _playerlisting;
    private List<PlayerListing> _listings = new List<PlayerListing>();
    
    void Awake(){
GetCurrentRoomPlayer();

    }
    private void GetCurrentRoomPlayer(){

        foreach(KeyValuePair<int,Player> playerinfo in PhotonNetwork.CurrentRoom.Players){

             AddPlayerListing(playerinfo.Value);
        }
    }

    private void AddPlayerListing(Player player){

            PlayerListing listing = Instantiate(_playerlisting,_content);
           if(listing !=null){
               listing.SetPlayerInfo(player);
               _listings.Add(listing);
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
    }
  
}
