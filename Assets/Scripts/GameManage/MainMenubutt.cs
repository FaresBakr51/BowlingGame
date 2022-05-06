using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenubutt : MonoBehaviourPunCallbacks
{

    public void GoHombutt() {

        if (!PhotonNetwork.OfflineMode || PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)
        {
            StartCoroutine(wait());
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
   
    private IEnumerator wait()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.AutomaticallySyncScene = false;
        while (PhotonNetwork.InRoom) {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }
   
}
