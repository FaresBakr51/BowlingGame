using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class CreatRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _roommax;
  [SerializeField]  private string _roomname;

     private GetNickname _getRoomname;
    // Start is called before the first frame update
    private RoomsController _roomController;
    public void FirstIniatlize(RoomsController _controll){
        _roomController  = _controll;
    }
void start(){
    _getRoomname = GetComponent<GetNickname>();

    StartCoroutine(getmyname());
}

 IEnumerator getmyname()
    {
        yield return new WaitForSeconds(2f);
       _roomname = GetNickname.nickname;
    }
    public void OnCreatRoom(){

       if(!PhotonNetwork.IsConnected)
       return;
        RoomOptions roomOptions = new RoomOptions();

        if(_roommax.text !=null && (int.Parse(_roommax.text)!= 2 ||int.Parse(_roommax.text)!= 4)){
        roomOptions.MaxPlayers = byte.Parse(_roommax.text);
        }else{
              roomOptions.MaxPlayers = 4;
        }
        PhotonNetwork.JoinOrCreateRoom(_roomname,roomOptions,TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    { 
        
       
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("room created");
      _roomController.CurrentRoomCanavas.Show();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("room failed creat");
    }
}
