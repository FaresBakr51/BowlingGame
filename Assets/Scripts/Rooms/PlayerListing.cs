using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
public class PlayerListing : MonoBehaviour
{
    public Player Player {get;private set;}

  
    [SerializeField] private  TextMeshProUGUI _text;
    [SerializeField] private GetNickname _getMyName;
    void Awake(){

        _getMyName = FindObjectOfType<GetNickname>();
    }
    public void SetPlayerInfo(Player player){

    Player = player;
    
   // player.NickName = PhotonNetwork.NickName;
 //   Player.NickName = PhotonNetwork.NickName;
 //player.NickName = Random.Range(5,666).ToString();
   _text.text  = player.NickName;
   }
  
}
