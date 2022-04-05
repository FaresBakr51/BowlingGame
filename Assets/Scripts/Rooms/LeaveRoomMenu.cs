using UnityEngine;
using Photon.Pun;
public class LeaveRoomMenu : MonoBehaviour
{

    private RoomsController _controller;
   [SerializeField] private GameObject _currentroompanel;
   public MainMenuManager _menuManager;
   public void Onclick_Leavroom(){
        if (GameModes._battleRoyale)
        {
            GameModes._battleRoyale = false;
        }
       PhotonNetwork.LeaveRoom(true);
       _controller.CurrentRoomCanavas.Hide();
       _menuManager.ActivealreadyRoompanel();
      
   }
   public void FirstIniatliz(RoomsController controll){

       _controller = controll;
   }
}
