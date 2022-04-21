using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

public class PlayerListing : MonoBehaviourPun
{
    public Player Player {get;private set;}
    public ExitGames.Client.Photon.Hashtable myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    public Image _platformImage;
    

    [SerializeField] private  TextMeshProUGUI _text;
    [SerializeField] private Sprite[] _plaformSprites;

    private object _temp;
    public bool _ImageSet;

    public void SetPlayerInfo(Player player){

    Player = player;
   _text.text  = player.NickName;

       
   
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
         
            myCustomProperties.Add("platformimage",0);
            PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
 
            myCustomProperties.Add("platformimage",1);
            PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
        }
     

    }

    private void Update()
    {
       PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("platformimage", out _temp);
        if ((int)_temp >= 0)
        {
         
            _platformImage.sprite = _plaformSprites[(int)_temp];
        }
    }
}
