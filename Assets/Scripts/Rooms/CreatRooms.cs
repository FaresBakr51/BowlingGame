using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class CreatRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _roommax;
     [SerializeField] private TMP_InputField _roomname;
 

     private GetNickname _getRoomname;
    // Start is called before the first frame update
    private RoomsController _roomController;
    [SerializeField] private GameObject _roomspanel;
    public MainMenuManager _menumanager;
    public void FirstIniatlize(RoomsController _controll){
        _roomController  = _controll;
    }
void start(){
    _getRoomname = GetComponent<GetNickname>();
    
   
}


    public void OnCreatRoom(){

       if(!PhotonNetwork.IsConnected)
       return;
       if(!PhotonNetwork.InLobby)
       return;
        RoomOptions roomOptions = new RoomOptions();
        
        roomOptions.MaxPlayers = 8;
          PhotonNetwork.JoinOrCreateRoom(GetNickname.nickname,roomOptions,TypedLobby.Default);
          

     /*    if(_roommax.text !="" && (int.Parse(_roommax.text)!= 2 ||int.Parse(_roommax.text)!= 4)){
        roomOptions.MaxPlayers = byte.Parse(_roommax.text);
        }
        if(_roommax.text == ""){

              roomOptions.MaxPlayers = 8;
        }


        
        if(_roomname.text !=""){
        PhotonNetwork.JoinOrCreateRoom(_roomname.text,roomOptions,TypedLobby.Default);
        }
        if(_roomname.text == ""){

               PhotonNetwork.JoinOrCreateRoom(GetNickname.nickname,roomOptions,TypedLobby.Default);
        } */
        
    }
  
    void Update(){
           if(SceneManager.GetActiveScene().name == "MainMenu"  && _roomspanel.activeInHierarchy == true){
           /*     if (Input.GetButtonDown("trianglebutton"))
            {
                if(!PhotonNetwork.InRoom){
                OnCreatRoom();
               
                }
            
            } */

        }
       
    }
    public override void OnJoinedRoom()
    { 
        
       
    }

    public override void OnCreatedRoom()
    {

        if(PhotonNetwork.OfflineMode == false){

        Debug.Log("room created");
      _roomController.CurrentRoomCanavas.Show();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
      OnCreatRoom();
    }
}
