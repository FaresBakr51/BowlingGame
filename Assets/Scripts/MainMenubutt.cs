using Photon.Pun;
public class MainMenubutt : MonoBehaviourPunCallbacks
{
    
    public void GoHombutt(){
if(PhotonNetwork.InRoom){
        PhotonNetwork.LeaveRoom();
}
        PhotonNetwork.LoadLevel(0);
    }
}
