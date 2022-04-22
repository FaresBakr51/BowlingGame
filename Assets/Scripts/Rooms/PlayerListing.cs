using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

public class PlayerListing : MonoBehaviourPun
{
    public Player Player {get;private set;}
  

    public Image _platformImage;
    

    [SerializeField] private  TextMeshProUGUI _text;
    [SerializeField] private Sprite[] _plaformSprites;

    private object _temp;
    private object other_temp;
    public bool _ImageSet;

    public void SetPlayerInfo(Player player){

        Player = player;
        _text.text  = player.NickName;

        //if(cuustomprop.TryGetValue("platformimage",out _temp)){

        //    _platformImage.sprite = _plaformSprites[(int)_temp];
        //}
        //var others = PhotonNetwork.PlayerListOthers;
      
        //var hash = player.CustomProperties;
        //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        //{

        //    hash.Add("platform", 0);
        //}
        //else if (Application.platform == RuntimePlatform.Android)
        //{
        //    hash.Add("platform", 1);
        //}
        //player.SetCustomProperties(hash);
    
        //if (hash.TryGetValue("platform", out _temp))
        //{
            
        //    _platformImage.sprite = _plaformSprites[(int)_temp];
        //}

        //for(int i = 0; i < others.Length; i++)
        //{
        //    Debug.Log(others[i].CustomProperties);
        //}

      



        //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    Debug.Log("Windows");
        //    if (cuustomprop.TryGetValue("platformimagepc", out _temp))
        //    {
        //        _platformImage.sprite = _plaformSprites[(int)_temp];
        //    }

        //}
        //else if (Application.platform == RuntimePlatform.Android)
        //{
        //    Debug.Log("arcade");
        //    if (cuustomprop.TryGetValue("platformimageandroid", out _temp))
        //    {

        //        _platformImage.sprite = _plaformSprites[(int)_temp];
        //    }

        //}


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

        //if (player.CustomProperties.TryGetValue("platformimage", out _temp))
        //{
        //    _platformImage.sprite = _plaformSprites[(int)_temp];
        //}
    }

    private void Update()
    {
        //if (Player.CustomProperties.TryGetValue("platformimage", out _temp))
        //{
        //    _platformImage.sprite = _plaformSprites[(int)_temp];
        //}

    }
}
