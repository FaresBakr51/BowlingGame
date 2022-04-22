using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System.Linq;
public class PlayerListing : MonoBehaviourPun
{
    public Player Player {get;private set;}
  
    [SerializeField] private  TextMeshProUGUI _text;

    public void SetPlayerInfo(Player player){
        Player = player;
        _text.text = player.NickName;


        //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    _platformImage.sprite = _plaformSprites[0];
        //    myCustomProperties.Clear();
        //    myCustomProperties.Add("platformimage",0);
        //    player.SetCustomProperties(myCustomProperties);
        //}
        //else if (Application.platform == RuntimePlatform.Android)
        //{
        //    _platformImage.sprite = _plaformSprites[1];
        //    myCustomProperties.Clear();
        //    myCustomProperties.Add("platformimage",1);
        //    player.SetCustomProperties(myCustomProperties);
        //}

    
    }

 
}
