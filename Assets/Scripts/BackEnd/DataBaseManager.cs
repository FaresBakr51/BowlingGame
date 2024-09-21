using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Steamworks;
using Firebase.Database;
using System;
using System.Linq;
using UnityEngine.Events;


namespace BackEnd
{
    public class DataBaseManager : Singelton<DataBaseManager>
    {
        private static DatabaseReference DbReference;

        private const string GameDBName = "BSOB";
        public static string UserID;
        public static string UserName;
        #region Google
      
        public static UnityEvent<string> SignIn = new UnityEvent<string>();
        public static UnityEvent SuccessSignIn = new UnityEvent();
        #endregion
        public bool Steam;
        public static PlayerData playerData;


        [SerializeField] private bool isLocallSaving;
        public bool IsLocallSaving { get { return isLocallSaving; } }
        private void OnEnable()
        {
           SignIn.AddListener(CheckUserName);
        }
        private void OnDisable()
        {
            SignIn.RemoveAllListeners();
        }
        private void Start()
        {
            //DbReference = FirebaseDatabase.DefaultInstance.RootReference;
            DontDestroyOnLoad(gameObject);
            if (Steam)
            {
                if (UserName == null)
                {
                    print("Checking Data Reconnecting ...");
                    SetSteamUserNmae();
                }
            }
       
        }
        #region SteamUsername
        public void SetSteamUserNmae()
        {
          //  steamManage.SetActive(true);
            if (SteamManager.Initialized)
            {
               // string name = SteamFriends.GetPersonaName();
                // string name = SteamFriends.GetPersonaName();
                Debug.Log(name);
                CheckUserName(name);
            }
            else
            {
             //   MainMenuManager.Instance.AlertUser("Couldn't Fetch Data !", () => { Application.Quit(); }, false);
            }
        }



        #endregion

        public void CheckUserName(string uid)
        {
           
            Debug.LogError(uid);
            // PlayerLocalStaticData.localPlayeruserName = user;
             StartCoroutine(CheckIfUsernameExists(uid));
            //UserName = user;
        }

 
        #region FetchFireBaseData
        public IEnumerator CheckIfUsernameExists(string uid )
        {
            Debug.LogError("CheckingUserNameCourtine");
            if (DbReference == null) DbReference = FirebaseDatabase.DefaultInstance.RootReference;
            var taskCheckuSER = DbReference.Child("Users").Child(uid).GetValueAsync();
            yield return new WaitUntil(predicate: () => taskCheckuSER.IsCompleted);
            if (taskCheckuSER.IsFaulted)
            {
                Debug.LogError(taskCheckuSER.Result.ToString() + "faulted");
                Debug.Log("not exist");
                //userNotexist
            }
            else if (taskCheckuSER.IsCompleted)
            {
                DataSnapshot snapshot = taskCheckuSER.Result;
                if (snapshot.Exists)//!= null)
                {
                    Debug.LogError("user exist");
                    //GetPlayerData
                    StartCoroutine(GetPlayerUserName(uid));
                  
                


                }
                else
                {
                    Debug.LogError("not exist");
                }
            }
            else if (taskCheckuSER.IsCanceled)
            {
                Debug.LogError("not exist");
                Application.Quit();
                //userNotexist
            }

        }//  DataBaseManager.SignIn?.Invoke(User.DisplayName);

        public IEnumerator RegisterNewUser(string uid,string username)
        {
            DbReference = FirebaseDatabase.DefaultInstance.RootReference;
            var tasknewuser = DbReference.Child("Users").Child(uid).Child("username").SetValueAsync(username);

            yield return new WaitUntil(predicate: () => tasknewuser.IsCompleted);

            if (tasknewuser.Exception != null)
            {


                Debug.Log(message: $"Failed to register task with {tasknewuser.Exception}");
                Application.Quit();

            }
            else
            {

                //REGISTER NEW USAER with empty data
                UserID = uid;
                UserName = username;
                StartCoroutine(GetPlayerData(uid));
            }

        }
        public void RegisterLocallUser(string username)//Overload for locall saving
        {
            isLocallSaving = true;
            if (PlayerPrefs.HasKey("username"))//check username (key)
            {
                UserName = PlayerPrefs.GetString("username");//load key
            }
            else
            {
                UserName = username;
                PlayerPrefs.SetString("username", username);
            }


            SuccessSignIn?.Invoke();
        }

        public static IEnumerator AddIndividualDataToUser<T>(string uid,string username, string childName, string childtype, T value )
        {
            Debug.LogError(uid);
            Debug.LogError(username);
            Debug.LogError("Parent = " + childName);
            var taskAddData = DbReference.Child("Users").Child(uid).Child(GameDBName).Child(childName).Child(childtype).SetValueAsync(value);

            yield return new WaitUntil(predicate: () => taskAddData.IsCompleted);

            if (taskAddData.Exception != null)
            {


                Debug.Log(message: $"Failed to register task with {taskAddData.Exception}");

            }
            else
            {
                // get_data(username,"playerData");

                Debug.Log("Registered Data Success");
               
               
            }

        }
    
    
        private IEnumerator GetPlayerUserName(string uid)
        {
            Debug.LogError("GettingPlayerData");
            var taskfetchdata = DbReference.Child("Users").Child(uid).Child("username").GetValueAsync();
            yield return new WaitUntil(predicate: () => taskfetchdata.IsCompleted);
            print("GettingPlayerData");
            if (taskfetchdata.Exception != null)
            {


                Debug.Log(message: $"Failed to register task with {taskfetchdata.Exception}");

            }
            else
            {
                var value = taskfetchdata.Result.Value;
                UserName = value.ToString();
                UserID = uid;
                StartCoroutine(GetPlayerData(uid));
                Debug.LogError("SetuserProfile");
            }
        }
        private IEnumerator GetPlayerData(string uid)
        {
            Debug.LogError("GettingPlayerData");
            var taskfetchdata = DbReference.Child("Users").Child(uid).Child(GameDBName).Child("playerData").GetValueAsync(); //Child(childN).GetValueAsync();
            yield return new WaitUntil(predicate: () => taskfetchdata.IsCompleted);
            print("GettingPlayerData");
            if (taskfetchdata.Exception != null)
            {


                Debug.Log(message: $"Failed to register task with {taskfetchdata.Exception}");

            }
            else
            {
                BashAuth.Instance.AuthAction = false;
                var value = taskfetchdata.Result.GetRawJsonValue();
                if (value == null)
                {

                    //there is no data registered for this user on this game
                    PlayerData data = new PlayerData();
                    data.InitializeEmptyData();
                    playerData = data;
                    StartCoroutine(AddPlayerGameData(uid, JsonUtility.ToJson(data)));
                }
                else
                {
                    playerData = JsonUtility.FromJson<PlayerData>(value);
                    //success gett data
                    if (playerData != null)
                    {
                        SuccessSignIn?.Invoke();
                    }
                }
                Debug.LogError(value);
            }

        }
        public static IEnumerator AddPlayerGameData(string uid, string jsondata)
        {
            var taskAddData = DbReference.Child("Users").Child(uid).Child(GameDBName).Child("playerData").SetRawJsonValueAsync(jsondata);

            yield return new WaitUntil(predicate: () => taskAddData.IsCompleted);

            if (taskAddData.Exception != null)
            {


                Debug.Log(message: $"Failed to register task with {taskAddData.Exception}");

            }
            else
            {
                // get_data(username,"playerData");

                Debug.Log("Registered new Data Success");
                SuccessSignIn?.Invoke();
                //do game function ex Connect to server


            }
        }
        #endregion
        #region LeaderBoard
        public static IEnumerator GetLoadLeaderboard()
        {
            var taskfetchdata = DbReference.Child("Users").GetValueAsync();
            yield return new WaitUntil(predicate: () => taskfetchdata.IsCompleted);
            print("GettingPlayerData");
            if (taskfetchdata.Exception != null)
            {


                Debug.Log(message: $"Failed to register task with {taskfetchdata.Exception}");

            }
            else
            {
          
                DataSnapshot snapshot = taskfetchdata.Result;//uid's
                List<string> usernames = new List<string>();
                List<int> scores = new List<int>();
    
                Dictionary<string, int> playersDict = new Dictionary<string, int>();
                Dictionary<string, object> uidsGameData = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> results in snapshot.Value as Dictionary<string, object>)
                {
                    if (results.Value != null)
                    {
                        foreach (KeyValuePair<string, object> uidsData in results.Value as Dictionary<string, object>)//fetch uid data
                        {
                            if(uidsData.Key == GameDBName)
                            {
                                //filter uid with gamedata
                                uidsGameData.Add(results.Key, results.Value);
                            }
                        }
                    }
                }
                if (uidsGameData.Count > 0)
                {
                    foreach (KeyValuePair<string, object> uid in uidsGameData)//fetch uid data
                    {
                        //on each uid
                        Debug.Log(uid.Key);
                        foreach (KeyValuePair<string, object> uidata in uid.Value as Dictionary<string, object>)//fetch uid data
                        {
                            if (uidata.Key == "username" && playersDict.Count < 8)
                            {
                                usernames.Add(uidata.Value.ToString());

                            }
                            if (uidata.Key == GameDBName)
                            {
                                foreach (KeyValuePair<string, object> gamedata in uidata.Value as Dictionary<string, object>)
                                {

                                    if (gamedata.Key == "playerData" && playersDict.Count < 8)
                                    {
                                        foreach (KeyValuePair<string, object> playerswins in gamedata.Value as Dictionary<string, object>)
                                        {
                                            if (playerswins.Key == "rankedPoints")
                                            {

                                                print(playerswins.Value);
                                                print(playerswins.Value.GetType());
                                                scores.Add((int)(Int64)playerswins.Value);

                                            }
                                        }

                                    }
                                }
                            }
                        }
                   
                    }
                }
                Debug.Log(usernames.Count);
                Debug.Log(scores.Count);
                Debug.Log(usernames.Count == scores.Count);
                if (usernames.Count == scores.Count)
                {
                    for(int i = 0; i < usernames.Count;)
                    {
                        if (playersDict.Count < 8)
                        {
                            if (!playersDict.ContainsKey(usernames[i]))
                            {
                                if (scores[i] != 0)
                                {
                                    playersDict.Add(usernames[i], scores[i]);
                                    Debug.Log("Adding scores");
                                    i++;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                var list = playersDict.OrderByDescending(x => x.Value).ToList();
                Debug.Log(list.Count);
                var count = 0;
                for(int i = 0; i < list.Count; i++)
                {
                   
                    if (count < 8)
                    {
                        
                        print(list[i].Value);
                        count++;
                        GameObject leaderboardobj = UnityEngine.Object.Instantiate(UiManager.Instance.leaderBoardPrefab, UiManager.Instance.ContentLeaderboard);
                         leaderboardobj.GetComponent<playerLeaderboardData>().SetData(list[i].Key, list[i].Value,i);
                    }
                }


            }
        }
        #endregion
    }

}