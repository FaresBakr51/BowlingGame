using System.Collections;
using UnityEngine;

using UnityEngine.Networking;
using System.Net.NetworkInformation;

public class GetNickname : MonoBehaviour {

    string server_url = "http://score.iircade.com/ranking/get_nickname.php";

    public static string device_id;
    public static string nickname;

    //---------------------------------------------------
    // Awake
    //---------------------------------------------------
    void Awake() {
        StartCoroutine(GetUserNickname());
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

        // Form Data Settings
        WWWForm form = new WWWForm();
        form.AddField("request", "get");
        form.AddField("device_id", device_id);

        using (UnityWebRequest request = UnityWebRequest.Post(server_url, form)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError) {
                Debug.Log("Network Error");
                yield break;
            }

            nickname = request.downloadHandler.text;
            Debug.Log($"{device_id} : {nickname}");
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
