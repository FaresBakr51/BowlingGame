using Firebase.Database;
using Photon.Pun;
using System.Collections;
using UnityEngine;


public class DBManager : MonoBehaviour
{
    private DatabaseReference DbReference;
    private void OnEnable()
    {
        DbReference = FirebaseDatabase.DefaultInstance.RootReference;
     

    }

    public IEnumerator CheckIfUsernameExists(string username)
    {
        var taskCheckuSER = DbReference.Child("Users").Child(username).GetValueAsync();
        yield return new WaitUntil(predicate: () => taskCheckuSER.IsCompleted);
        if (taskCheckuSER.IsFaulted)
        {
            Debug.LogError(taskCheckuSER.Result.ToString() + "faulted");
            Debug.Log("not exist faild");
          
            //userNotexist
        }
        else if (taskCheckuSER.IsCompleted)
        {
            DataSnapshot snapshot = taskCheckuSER.Result;
            if (snapshot.Exists)//!= null)
            {
                Debug.Log("user exist");
                MainMenuAndNetworkManager.GetRankedPointsAction?.Invoke();
                MainMenuAndNetworkManager.Instance._setNamePanel.SetActive(false);
                MainMenuAndNetworkManager.Instance._mainPanel.SetActive(true);
                MainMenuAndNetworkManager.Instance.SetSelectedGameObject(MainMenuAndNetworkManager.Instance.mainButtons[0]);
                if (!SubscriptionManager.ISLocalUserRegistered())
                {
                    SubscriptionManager.SubsButtonsState?.Invoke(true);
                }
                StartCoroutine(GetData("Users", username, "tokenData"));
            }
            else
            {
                Debug.Log("not exist");

                StartCoroutine(RegisterNewUser(username));
                //userNotexist

            }
        }
        else if (taskCheckuSER.IsCanceled)
        {
            Debug.Log("not exist");
            //userNotexist
        }

    }
    public IEnumerator RegisterNewUser(string username)
    {
        DbReference = FirebaseDatabase.DefaultInstance.RootReference;
        var tasknewuser = DbReference.Child("Users").Child(username).SetValueAsync(username);

        yield return new WaitUntil(predicate: () => tasknewuser.IsCompleted);

        if (tasknewuser.Exception != null)
        {


            Debug.Log(message: $"Failed to register task with {tasknewuser.Exception}");

        }
        else
        {

            //meta data
            Debug.Log("Registered Success");
            MainMenuAndNetworkManager.GetRankedPointsAction?.Invoke();
            MainMenuAndNetworkManager.Instance._setNamePanel.SetActive(false);
            MainMenuAndNetworkManager.Instance._mainPanel.SetActive(true);
            MainMenuAndNetworkManager.Instance.SetSelectedGameObject(MainMenuAndNetworkManager.Instance.mainButtons[0]);
            SubscriptionManager.SubsButtonsState?.Invoke(true);
            //getuserData if tokens Exist


        }

    }

    public void RegisterToken(string username,string jsondata)
    {
        StartCoroutine(SetGlobalData("Users", username, "tokenData", jsondata));
    }
    public IEnumerator SetGlobalData(string mainChild, string childN1, string childN2, string rawJsonData)
    {
        var taskfetchdata = DbReference.Child(mainChild).Child(childN1).Child(childN2).SetRawJsonValueAsync(rawJsonData);
        yield return new WaitUntil(predicate: () => taskfetchdata.IsCompleted);

        if (taskfetchdata.Exception != null)
        {


            Debug.Log(message: $"Failed to register task with {taskfetchdata.Exception}");

        }
        else
        {
            Debug.Log("completed");
            Debug.Log(rawJsonData);

        }
    }
    public IEnumerator GetData(string mainChild, string childN1, string childN2)
    {
        var taskfetchdata = DbReference.Child(mainChild).Child(childN1).Child(childN2).GetValueAsync();
        yield return new WaitUntil(predicate: () => taskfetchdata.IsCompleted);

        if (taskfetchdata.Exception != null)
        {


            Debug.Log(message: $"Failed to register task with {taskfetchdata.Exception}");

        }
        else
        {
            if (taskfetchdata.Result.GetRawJsonValue() != null)
            {
                SubscriptionManager.MyToken = JsonUtility.FromJson<TokenSubscription>(taskfetchdata.Result.GetRawJsonValue());
                if (SubscriptionManager.MyToken != null)
                {
                    if (SubscriptionManager.MyToken.tokenkey == "weekly")
                    {
                        if (SubscriptionManager.GetDaysPassed(System.Convert.ToDateTime(SubscriptionManager.MyToken.purchasetime)) >= 7)
                        {
                            RegisterToken(PhotonNetwork.LocalPlayer.NickName, null);
                            SubscriptionManager.MyToken = null;
                        }
                        else
                        {
                            //can restore
                            SubscriptionManager.SubsButtonsState?.Invoke(true);
                            //continueSubscription
                        }
                    }
                    else
                    {
                        //can restore full
                        SubscriptionManager.SubsButtonsState?.Invoke(true);
                    }
                }
                else
                {
                    Debug.Log("Data Null");
                }
            }
            else
            {
                Debug.Log("Active sub buttons");
                SubscriptionManager.SubsButtonsState?.Invoke(true);
            }
            Debug.Log(taskfetchdata.Result.GetRawJsonValue());
            //if(SubscriptionManager.MyToken !=null)Debug.Log(SubscriptionManager.MyToken.tokenkey);

        }
    }
    public static string GenerateRandomTokent()
    {
        string characters = "abcdefghigklmnobqrsxyuz1234567890";
        string randString = "";
        for(int i = 0; i < 8; i++)
        {
            randString += characters[UnityEngine.Random.Range(0,characters.Length)];
            
        }
      return randString;
    }
}
[System.Serializable]
public class TokenSubscription
{

    public string purchasetime;
    public string tokenkey;
    public string token;
    public TokenSubscription(string key,string purchasedTime,string tokenId)
    {
        purchasetime = purchasedTime;
        tokenkey = key;
        token = tokenId;
    }
    public TokenSubscription() { }
}
