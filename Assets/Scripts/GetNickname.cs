using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using Photon.Pun;
public class GetNickname : MonoBehaviour {

    string server_url = "http://score.iircade.com/ranking/get_nickname.php";

    public  string device_id;
    public   string nickname;
    public GameObject _mynameSet;
    [SerializeField] private InputField _NameInputfield;
   

    //---------------------------------------------------
    // Awake
    //---------------------------------------------------
    
    public void SetMyName(){

        nickname = _NameInputfield.text;
     PhotonNetwork.LocalPlayer.NickName = nickname;
     StartCoroutine(ShowMyname());
    }
    IEnumerator ShowMyname(){
        if(_NameInputfield.text != ""){
        _mynameSet.SetActive(true);
        yield return new WaitForSeconds(2);
        _mynameSet.SetActive(false);
        }

    }
    void Awake() {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(GetUserNickname());
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor )
        {
            PhotonNetwork.LocalPlayer.NickName = "Bowler" + Random.Range(100, 10000);
        }
      
        
    }
    void Start(){


      
    }

    // Update is called once per frame
    void Update() {

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
