using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.EventSystems;

public class GetNickname : MonoBehaviour {

    string server_url = "http://score.iircade.com/ranking/get_nickname.php";

    public  string device_id;
    public   string nickname;
    public GameObject _mynameSet;
    [SerializeField] private TMP_InputField _NameInputfield;
    public bool _IGT;
    public GameObject _MainPanel;
    public GameObject _SetNamebutt;

    //---------------------------------------------------
    // Awake
    //---------------------------------------------------
    
    public void SetMyName(){
        if (_NameInputfield.text != "")
        {
            nickname = _NameInputfield.text;
            PhotonNetwork.LocalPlayer.NickName = nickname;

            PlayerPrefs.SetString("myname", nickname);
            PlayerPrefs.Save();
            MainMenuAndNetworkManager.GetRankedPointsAction?.Invoke();
        }

  
    }

    void Awake() {
        if (!_IGT)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                StartCoroutine(GetUserNickname());
            }
            else if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)&&!_IGT)
            {
                Debug.Log("RandomBowler");
                PhotonNetwork.LocalPlayer.NickName = "Bowler" + Random.Range(100, 10000);
                nickname = PhotonNetwork.LocalPlayer.NickName;
                MainMenuAndNetworkManager.GetRankedPointsAction?.Invoke();
            }
        }
        else
        {
            Debug.Log("igt");
            if (PlayerPrefs.HasKey("myname"))
            {
                nickname = PlayerPrefs.GetString("myname");
                PhotonNetwork.LocalPlayer.NickName = nickname;
                MainMenuAndNetworkManager.GetRankedPointsAction?.Invoke();
            }
            else
            {
                _mynameSet.SetActive(true);
                _MainPanel.SetActive(false);
                EventSystem.current.SetSelectedGameObject(_SetNamebutt);

            }
        }
        
    }

    //---------------------------------------------------
    // Get User Nickname
    //
    //  return value 
    //    * : device not found
    //    - : has no nickname
    //  etc : user nickname 
    //
    //---------------------------------------------------
    IEnumerator GetUserNickname() {
        GetDeviceID();

        yield return new WaitForSeconds(2f);
        // Form Data Settings
        WWWForm form = new WWWForm();
        form.AddField("request", "get");
        form.AddField("device_id", device_id);

        using (UnityWebRequest request = UnityWebRequest.Post(server_url, form)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError) {
               
                yield break;
            }

            nickname = request.downloadHandler.text;
            PhotonNetwork.LocalPlayer.NickName = nickname;
            MainMenuAndNetworkManager.GetRankedPointsAction?.Invoke();
           //  _pv.Owner.NickName = nickname;
          
        }
    }

    //---------------------------------------------------
    // Get DeviceID <- GetUserNickname
    //---------------------------------------------------
    void GetDeviceID() {
        device_id = "";
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface adapter in nics) {
            PhysicalAddress address = adapter.GetPhysicalAddress();
            if (!address.ToString().Equals("")) {
                device_id = address.ToString().Substring(2);
                return;
            }
        }
    }

    //---------------------------------------------------
    // Test code
    //---------------------------------------------------


}
