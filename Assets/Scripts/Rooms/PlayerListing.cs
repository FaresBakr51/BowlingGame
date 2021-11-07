using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class PlayerListing : MonoBehaviour
{


    public Player Player {get;private set;}

  
    [SerializeField] private  TextMeshProUGUI _text;

    public void SetPlayerInfo(Player player){

    Player = player;
    int randname = Random.Range(0,6966);
   _text.text  =  GetNickname.nickname + randname;
   }
  
}
