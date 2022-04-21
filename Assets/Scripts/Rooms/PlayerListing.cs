using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
public class PlayerListing : MonoBehaviour
{
    public Player Player {get;private set;}

  
    [SerializeField] private  TextMeshProUGUI _text;
    [SerializeField] private Image _platformImage;
    [SerializeField] private Sprite[] _plaformSprites;
    public void SetPlayerInfo(Player player){

    Player = player;
   _text.text  = player.NickName;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            _platformImage.sprite = _plaformSprites[0];
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            _platformImage.sprite = _plaformSprites[1];
        }
    }
  
}
