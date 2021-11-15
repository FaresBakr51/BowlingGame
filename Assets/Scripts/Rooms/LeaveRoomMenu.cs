using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class LeaveRoomMenu : MonoBehaviour
{

    private RoomsController _controller;
   [SerializeField] private GameObject _currentroompanel;
   public void Onclick_Leavroom(){
       PhotonNetwork.LeaveRoom(true);
       _controller.CurrentRoomCanavas.Hide();
   }
   public void FirstIniatliz(RoomsController controll){

       _controller = controll;
   }

     void Update(){

         if(SceneManager.GetActiveScene().name == "MainMenu" && _currentroompanel.activeInHierarchy == true){
               if (Input.GetButtonDown("obutton"))
            {
                if(PhotonNetwork.InRoom){
                
                 Onclick_Leavroom();
                }
            
            }
        }
    }
   
}
