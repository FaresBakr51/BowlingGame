using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerListing _playerlisting;
    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsController _controller;
    [SerializeField] private GameObject _startButt;
    [SerializeField] private GameObject _currentroompanel;
    [SerializeField] private GameObject _WaitingPlayerMass;

    [SerializeField] private Sprite[] _plaformSprites;
    private bool _gameLoading;
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
    private void Start()
    {
        _gameLoading = false;
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

       /*  int index = _listings.FindIndex(x => x.Player == player);
        if(index != -1){
            _listings[index].SetPlayerInfo(player);
        }else{ */
            PlayerListing listing = Instantiate(_playerlisting,_content);
     
           if (listing !=null){
        
            //ExitGames.Client.Photon.Hashtable myCustomProperties = new ExitGames.Client.Photon.Hashtable();
            //myCustomProperties.Clear();
            //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            //{

            //    myCustomProperties.Add("platformimagepc", 0);
            //}
            //else if (Application.platform == RuntimePlatform.Android)
            //{
            //    myCustomProperties.Add("platformimageandroid", 1);
            //}
          
            //player.SetCustomProperties(myCustomProperties);
            listing.SetPlayerInfo(player);
               _listings.Add(listing);
           }
       // }
         if(!PhotonNetwork.IsMasterClient){
             _WaitingPlayerMass.SetActive(true);
             _startButt.SetActive(false);
        
         }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (GameModes._rankedMode) return;
         AddPlayerListing(newPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            if (!GameModes._battleRoyale)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                {
                    _WaitingPlayerMass.SetActive(false);
                    _startButt.SetActive(true);
                }
                if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
                {
                    _WaitingPlayerMass.SetActive(true);
                    _startButt.SetActive(false);
                }
            }
            else
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 3)
                {
                    _WaitingPlayerMass.SetActive(false);
                    _startButt.SetActive(true);
                }
                if (PhotonNetwork.CurrentRoom.PlayerCount < 3)
                {
                    _WaitingPlayerMass.SetActive(true);
                    _startButt.SetActive(false);
                }
            }
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (GameModes._rankedMode) return;
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
               if(index != -1){

                  Destroy(_listings[index].gameObject);
                   _listings.RemoveAt(index);
               }
                
    }
    public void Onclick_StartGame(){
        if (!GameModes._battleRoyale)
        {
            if (PhotonNetwork.IsMasterClient && !_gameLoading)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount > 6)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.LoadLevel(2);
                }
                else if (PhotonNetwork.CurrentRoom.PlayerCount <= 6)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.LoadLevel(Random.Range(2, 4));
                }

                _gameLoading = true;
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient && !_gameLoading && PhotonNetwork.CurrentRoom.PlayerCount >= 3)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel(5);
            }
            _gameLoading = true;
        }
    }

    private void Update()
    {
        if (_listings.Count > 0)
        {
             SetplayersPlatform();
            //if (TypePlatform() != -1 && !_listings[TypePlatform()]._ImageSet)
            //{

            //     _listings[TypePlatform()].GetComponentInChildren<Image>().sprite = _plaformSprites[TypePlatform()];
            //    _listings[TypePlatform()]._ImageSet = true;

            //}

        }

    }
    //int TypePlatform()
    //{
    //    object temp  = null;
    //    int isArcade = _listings.FindIndex(t => t.Player.CustomProperties.TryGetValue("platformimage",out temp));
    //    Debug.Log(temp);
    //    return (int)temp;

    //}

    private void SetplayersPlatform()
    {


        //for (int i = 0; i < _listings.Count; i++)
        //{
        //    object temp;
        //    if(_listings[i].Player.CustomProperties.TryGetValue("platformimage", out temp))
        //    {
        //        _listings[i].GetComponentInChildren<Image>().sprite = _plaformSprites[(int)(temp)];

        //    }
         
        //}

    }



        //}


  }
