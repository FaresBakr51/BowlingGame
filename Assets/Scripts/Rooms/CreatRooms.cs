using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class CreatRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _roommax;
     [SerializeField] private TMP_InputField _roomname;
     private GetNickname _getRoomname;
    private RoomsController _roomController;
    [SerializeField] private GameObject _roomspanel;
    [SerializeField] private GetNickname _getMyName;
    public void FirstIniatlize(RoomsController _controll){
        _roomController  = _controll;
    }


    public void OnCreatRoom(){
        if (GameModes._rankedMode) return;
        if (!PhotonNetwork.IsConnected) return;
      //  if (!PhotonNetwork.InLobby) return;
        RoomOptions roomOptions = new RoomOptions();
        
        roomOptions.MaxPlayers = 8;
        roomOptions.CleanupCacheOnLeave = false;
       PhotonNetwork.JoinOrCreateRoom(_getMyName.nickname,roomOptions,TypedLobby.Default);
      
        
    }
    public void OnCreatBattleMode()
    {
        
        GameModes._battleRoyale = true;
        if (!PhotonNetwork.IsConnected)
            return;
      //  if (!PhotonNetwork.InLobby)
     //       return;
        if (GameModes._battleRoyale)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 15;
            PhotonNetwork.JoinOrCreateRoom(_getMyName.nickname, roomOptions, TypedLobby.Default);
        }
    }

    public override void OnCreatedRoom()
    {

        if(!PhotonNetwork.OfflineMode && !GameModes._rankedMode)
        {
          _roomController.CurrentRoomCanavas.Show();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
      OnCreatRoom();
    }
}
