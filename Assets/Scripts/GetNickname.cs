using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.NetworkInformation;
using Photon.Pun;
using TMPro;
using UnityEngine.EventSystems;


//using Steamworks;
public class GetNickname : MonoBehaviour {

    string server_url = "http://score.iircade.com/ranking/get_nickname.php";

   
    public  string device_id;
    public   string nickname;
    public GameObject _mynameSet;
    [SerializeField] private TMP_InputField _NameInputfield;
    public bool _IGT;
    public GameObject _MainPanel;
    public GameObject _SetNamebutt;
    public GameObject steamApply;


    //---------------------------------------------------
    // Awake
    //---------------------------------------------------
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.steamBuild, SteamSetupName);
        GameEventBus.Subscribe(GameEventType.IGTbuild, IGTSetUpName);
        GameEventBus.Subscribe(GameEventType.PolyCadebuild, SetRandomNames);
        GameEventBus.Subscribe(GameEventType.XboxBuild, SetNameMenu);
        GameEventBus.Subscribe(GameEventType.ArcadeBuild, SetServerArcadeName);
    }
    private void OnDisable()
    {
        GameEventBus.UnSubscribe(GameEventType.steamBuild, SteamSetupName);
        GameEventBus.UnSubscribe(GameEventType.IGTbuild, IGTSetUpName);
        GameEventBus.UnSubscribe(GameEventType.PolyCadebuild, SetRandomNames);
        GameEventBus.UnSubscribe(GameEventType.XboxBuild, SetNameMenu);
        GameEventBus.UnSubscribe(GameEventType.ArcadeBuild, SetServerArcadeName);
    }

    private void SetServerArcadeName()
    {
        StartCoroutine(GetUserNickname());
    }
    private void SetNameMenu()
    {
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
    private void SteamSetupName()
    {
        steamApply.SetActive(true);
        
        if (!SteamManager.Initialized) return;
       // string name = SteamFriends.GetPersonaName();
        SetName(name);
    }
    private void IGTSetUpName()
    {
        Debug.Log("IGTSETUP");
    }
    private void SetRandomNames()
    {
        SetName("Bowler" + Random.Range(100, 10000));
    }
    public void SetMyNameFromGame(){
        if (_NameInputfield.text != "")
        {
            nickname = _NameInputfield.text;
            PhotonNetwork.LocalPlayer.NickName = nickname;
            PlayerPrefs.SetString("myname", nickname);
            PlayerPrefs.Save();

            MainMenuAndNetworkManager.GetRankedPointsAction?.Invoke();
            MainMenuAndNetworkManager.Instance._setNamePanel.SetActive(false);
            MainMenuAndNetworkManager.Instance._mainPanel.SetActive(true);
            MainMenuAndNetworkManager.Instance.SetSelectedGameObject(MainMenuAndNetworkManager.Instance.mainButtons[0]);
        }

  
    }
 


    public void SetName(string name)
    {

        nickname = name;
        PhotonNetwork.LocalPlayer.NickName = nickname;
        MainMenuAndNetworkManager.GetRankedPointsAction?.Invoke();
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
