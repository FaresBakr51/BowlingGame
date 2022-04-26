using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
public class PlayerListing : MonoBehaviourPun
{
    public Player Player {get;private set;}
  
    [SerializeField] private  TextMeshProUGUI _text;

    public void SetPlayerInfo(Player player){
        Player = player;
        _text.text = player.NickName;
    }

 
}
