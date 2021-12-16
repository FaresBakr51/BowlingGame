using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class MainMenubutt : MonoBehaviourPunCallbacks
{
    
    public void GoHombutt(){
if(PhotonNetwork.InRoom){
    PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
}
        PhotonNetwork.LoadLevel(0);
    }
   void Update(){

        if(SceneManager.GetActiveScene().name == "Map1" ){
        /*        if (Input.GetButtonDown("square"))
            {
               
                
                  GoHombutt();
                
            
            } */
        }
   }
}
