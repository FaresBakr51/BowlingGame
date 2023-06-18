using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using Photon.Pun;

public class SubscriptionManager : MonoBehaviour
{

    [SerializeField]
    private DBManager dbManager;
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


    private void Start()
    {
        if(PlayerPrefs.GetInt("gameweekly", 0) == 1)
        {
            CheckDate();
        }
    }
    void SetStartDate()
    {
        if (PlayerPrefs.HasKey("DateInitialized"))
        {
            startDate = System.Convert.ToDateTime(PlayerPrefs.GetString("DateInitialized"));
            if (GetDaysPassed() >= 7)
            {
                PlayerPrefs.SetInt("gameweekly", 0);
                PlayerPrefs.DeleteKey("DateInitialized");
            }
        }
        else //otherwise...
        {
            startDate = System.DateTime.Now; //save the start date ->
            PlayerPrefs.SetString("DateInitialized", startDate.ToString());
        }
      
    }

    private void CheckDate()
    {
        if (PlayerPrefs.HasKey("DateInitialized"))
        {
            startDate = System.Convert.ToDateTime(PlayerPrefs.GetString("DateInitialized"));
            Debug.Log(GetDaysPassed());
            if (GetDaysPassed() >= 7)
            {
                PlayerPrefs.SetInt("gameweekly", 0);
                PlayerPrefs.DeleteKey("DateInitialized");
            }
        }
    }
    public static double GetDaysPassed()
    {
        today = System.DateTime.Now;

        //days between today and start date -->
        System.TimeSpan elapsed = today.Subtract(startDate);

        double days = elapsed.TotalDays;

        return days;
    }

    public void OnSubscriptionComplete(Product product)
    {
        if(product.definition.id == "com.gameweeklysubscription")
        {
            //unlouck game
            PlayerPrefs.SetInt("gameweekly", 1);
            SetStartDate();
           StartCoroutine(dbManager.CheckIfTokenExist(DBManager.GenerateRandomTokent()));
        
            MainMenuAndNetworkManager.Instance.CheckPurchaseButtonsState();

        }
        if (product.definition.id == "com.gamefullsubscription")
        {
            //unlouck game
           
            PlayerPrefs.SetInt("gamefull", 1);
            MainMenuAndNetworkManager.Instance.CheckPurchaseButtonsState();

        }
    }
    


    public void LoadInterstitialAd()
    {
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
}

