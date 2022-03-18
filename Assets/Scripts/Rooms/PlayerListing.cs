using UnityEngine;
using TMPro;
using Photon.Realtime;
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
   _text.text  = player.NickName;
   }
  
}
