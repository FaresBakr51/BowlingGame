using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using Photon.Pun;
using TMPro;
public enum SubscriptionType
{
    weekly, Full
}

public class SubscriptionManager : MonoBehaviour
{

#if !UNITY_WEBGL
#if !UNITY_WEBGL
    [SerializeField]
    private DBManager dbManager;
#endif
    [Header("Ads")]
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-6154394693784009/7974347180";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif
    private InterstitialAd interstitialAd;

    private static System.DateTime startDate;
    private static System.DateTime today;

#if !UNITY_WEBGL
  public static TokenSubscription MyToken;
#endif
    [Header("SubscriptionButtons")]
    [SerializeField] private GameObject fullgameButton;
    [SerializeField] private GameObject weeklyButton;
    [SerializeField] private GameObject restoreButton;
    [SerializeField] private TMP_InputField tokenTxt;
    [SerializeField] private TextMeshProUGUI restoreTxterror;
    public static Action<bool> SubsButtonsState;
    private void OnEnable()
    {
        SubsButtonsState += SetSubsButtonsState;
        fullgameButton.SetActive(false);
        weeklyButton.SetActive(false);
        restoreButton.SetActive(false);
    }
    private void OnDisable()
    {
        SubsButtonsState -= SetSubsButtonsState;
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("gameweekly", 0) == 1)
        {
            CheckDate();
        }
    }
    void StartWeekSubscriptionDate(bool continu)
    {
        if (!continu)
        {
            if (PlayerPrefs.HasKey("DateInitialized"))
            {
                startDate = System.Convert.ToDateTime(PlayerPrefs.GetString("DateInitialized"));
                if (GetDaysPassed(startDate) >= 7)
                {
                    PlayerPrefs.SetInt("gameweekly", 0);
                    dbManager.RegisterToken(PhotonNetwork.LocalPlayer.NickName, JsonUtility.ToJson(new TokenSubscription()));
                    PlayerPrefs.DeleteKey("DateInitialized");
                }
            }
            else //otherwise...
            {
                startDate = System.DateTime.UtcNow; //save the start date ->
                PlayerPrefs.SetString("DateInitialized", startDate.ToString());
            }
        }
        else
        {
            
            PlayerPrefs.SetString("DateInitialized", MyToken.purchasetime.ToString());
            if (GetDaysPassed(System.Convert.ToDateTime(MyToken.purchasetime.ToString())) >= 7)
            {
                PlayerPrefs.SetInt("gameweekly", 0);
                dbManager.RegisterToken(PhotonNetwork.LocalPlayer.NickName, JsonUtility.ToJson(new TokenSubscription()));
                PlayerPrefs.DeleteKey("DateInitialized");
                restoreTxterror.text = "Subscription Expired !";
                SubsButtonsState?.Invoke(true);
            }

            //if (GetTotalMinutesPassed(System.Convert.ToDateTime(MyToken.purchasetime.ToString())) >= 1)
            //{
            //    Debug.Log(MyToken.purchasetime.ToString());
            //    Debug.Log(GetTotalMinutesPassed(System.Convert.ToDateTime(MyToken.purchasetime.ToString())));
            //    Debug.Log("Test Passed Minut Passed since last continued data saved from new machine");
            //    PlayerPrefs.SetInt("gameweekly", 0);
            //    dbManager.RegisterToken(PhotonNetwork.LocalPlayer.NickName, JsonUtility.ToJson(new TokenSubscription()));
            //    PlayerPrefs.DeleteKey("DateInitialized");
            //    restoreTxterror.text = "Subscription Expired !";
            //    SubsButtonsState?.Invoke(true);
            //}
        }
      
    }
   
    [ContextMenu("TestRemoveWeeklySubscribtion")]
    public void RemoveWeeklySubscription()
    {
        PlayerPrefs.SetInt("gameweekly", 0);
        dbManager.RegisterToken(PhotonNetwork.LocalPlayer.NickName, JsonUtility.ToJson(new TokenSubscription()));
        PlayerPrefs.DeleteKey("DateInitialized");
    }
    public void CheckDate()
    {
        if (PlayerPrefs.HasKey("DateInitialized"))
        {
            startDate = System.Convert.ToDateTime(PlayerPrefs.GetString("DateInitialized"));
            Debug.Log(GetDaysPassed(startDate));
            if (GetDaysPassed(startDate) >= 7)
            {
                PlayerPrefs.SetInt("gameweekly", 0);
                dbManager.RegisterToken(PhotonNetwork.LocalPlayer.NickName, JsonUtility.ToJson(new TokenSubscription()));
                PlayerPrefs.DeleteKey("DateInitialized");
            }
        }
    }
    public static double GetDaysPassed(DateTime startDate)
    {
        today = System.DateTime.UtcNow;
     
        //days between today and start date -->
        System.TimeSpan elapsed = today.Subtract(startDate);

        double days = elapsed.TotalDays;

        return days;
    }
    //public static double GetTotalMinutesPassed(DateTime startDate)
    //{
    //    today = System.DateTime.UtcNow;
    //    Debug.Log("Today time " + today);
    //    //days between today and start date -->
    //    System.TimeSpan elapsed = today.Subtract(startDate);

    //    double days = elapsed.TotalMinutes;

    //    return days;

    //}

    public void OnSubscriptionComplete(Product product)
    {
        string randToken = DBManager.GenerateRandomTokent();
        TokenSubscription tokenSubscription;
        if (product.definition.id == "com.gameweeklysubscription")
        {
            tokenSubscription = new TokenSubscription("weekly", DateTime.UtcNow.ToString(), randToken);
            MyToken = tokenSubscription;
            dbManager.RegisterToken(PhotonNetwork.LocalPlayer.NickName, JsonUtility.ToJson(tokenSubscription));
            //unlouck game
            ActiveSpecifecSubscription(SubscriptionType.weekly,false);

        }
        if (product.definition.id == "com.gamefullsubscription")
        {
            //unlouckfull  game


            tokenSubscription = new TokenSubscription("full", DateTime.UtcNow.ToString(), randToken);
            MyToken = tokenSubscription;
            dbManager.RegisterToken(PhotonNetwork.LocalPlayer.NickName, JsonUtility.ToJson(tokenSubscription));
            ActiveSpecifecSubscription(SubscriptionType.Full,false);
        }
      
    }
    public void ActiveSpecifecSubscription(SubscriptionType type,bool contin)
    {
      
        switch (type)
        {
            case SubscriptionType.Full:
                PlayerPrefs.SetInt("gamefull", 1);
                SubsButtonsState?.Invoke(false);
                break;
            case SubscriptionType.weekly:
                PlayerPrefs.SetInt("gameweekly", 1);
                SubsButtonsState?.Invoke(false);
                StartWeekSubscriptionDate(contin);
                break;
        }
      

    }
    
    public static bool ISLocalUserRegistered()
    {
        return PlayerPrefs.GetInt("gamefull") == 1 || PlayerPrefs.GetInt("gameweekly") == 1;
    }

    public void LoadInterstitialAd()
    {
        if (!MainMenuAndNetworkManager.Instance || MainMenuAndNetworkManager.Instance.gamePlatform != GameEventType.AndroidBuild) return;
        // Clean up the old ad before loading a new one.
        if (PlayerPrefs.GetInt("gamefull", 0) != 1 && PlayerPrefs.GetInt("gameweekly", 0) != 1)
        {

            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();
            // send the request to load the ad.
            InterstitialAd.Load(_adUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    interstitialAd = ad;
                    ShowAdd();
                });
        }
    }
    public void ShowAdd()
    {
        
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        
    }
    public void ShowRestoreButt()
    {
        if (MyToken != null)
        {
            tokenTxt.text = MyToken.token;
            Debug.Log(tokenTxt.text);
            Debug.Log(MyToken.token);
            //check if token is valid
        }
    }
    public void RestoreButt()
    {
        if (tokenTxt.text != "")
        {
            if (tokenTxt.text == MyToken.token)
            {

                if (MyToken.tokenkey == "weekly")
                {
                    restoreTxterror.text = "Restored Success !";
                    ActiveSpecifecSubscription(SubscriptionType.weekly, true);
                  
                }
                else
                {
                    restoreTxterror.text = "Restored Success !";
                    ActiveSpecifecSubscription(SubscriptionType.Full, false);
                   
                }
            }
        }
    }
    public void SetSubsButtonsState(bool state)
    {
        fullgameButton.SetActive(state);
        weeklyButton.SetActive(state);
        restoreButton.SetActive(state);
    }
#endif
}

