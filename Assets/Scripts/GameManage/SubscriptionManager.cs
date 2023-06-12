using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class SubscriptionManager : MonoBehaviour
{
    DateTime startime;
    private TimeSpan cycle;
    private SubscriptionInfo SubscriptionInfo;
    
    void OnApplicationQuit()
    {
        PlayerPrefs.SetString("timesaved", PlayerPrefs.GetString("timesaved"));
    }
    void Start()
    {
        startime = DateTime.Now;


        if (PlayerPrefs.HasKey("timesaved"))
        {
            cycle = startime.Subtract(DateTime.Parse(PlayerPrefs.GetString("timesaved")));

            Debug.Log(cycle.TotalMinutes);
            if (cycle.TotalMinutes >= 720)
            {


               //reset subscription
                PlayerPrefs.SetString("timesaved", startime.ToString());
            }



        }
        else
        {
            PlayerPrefs.SetString("timesaved", startime.ToString());

        }


    }
    public void OnSubscriptionComplete(Product product)
    {
        if(product.definition.id == "com.gameSubscription")
        {
            //unlouck game
            PlayerPrefs.SetInt("gameweekly", 1);
            MainMenuAndNetworkManager.Instance.PlayNextMainButtAnimation();
            SubscriptionInfo = new SubscriptionInfo(product.definition.id);

        }
        if (product.definition.id == "com.gamefullSubscription")
        {
            //unlouck game
           
            PlayerPrefs.SetInt("gamefull", 1);
            MainMenuAndNetworkManager.Instance.PlayNextMainButtAnimation();

        }
    }
    public SubscriptionInfo GetSubInfo(string id)
    {
        return SubscriptionInfo;
    }



}
