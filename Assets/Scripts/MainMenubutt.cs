using Photon.Pun;
public class MainMenubutt : MonoBehaviourPunCallbacks
{
    
    public void GoHombutt(){
if(PhotonNetwork.InRoom){
 //   PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
}
        PhotonNetwork.LoadLevel(0);
    }
}
