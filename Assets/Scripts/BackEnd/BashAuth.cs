#if !UNITY_WEBGL
using Firebase.Auth;
using Firebase.Extensions;
#endif
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BackEnd
{
    public class BashAuth : AuthManager
    {
        #region EmailAuth
        //private FirebaseAuth auth;
        //private FirebaseUser User;

        [SerializeField] private TextMeshProUGUI successMessage;
        [SerializeField] private TextMeshProUGUI[] errormessages;


        [SerializeField] private Toggle rememberMe;
        [SerializeField] private GameObject steamApply;

        [SerializeField] private GameObject bashPanel;






        #region Platforms 
        [SerializeField] private GameObject[] Modes;
        #endregion

        private void OnEnable()
        {
            GameEventBus.Subscribe(GameEventType.arcademode, OnSignInIIRCade);
#if !UNITY_WEBGL
            GameEventBus.Subscribe(GameEventType.PolyCadebuild, OnSignInPolyCade);
          //  GameEventBus.Subscribe(GameEventType.steamBuild, OnSteamSignIn);
#endif
            GameEventBus.Subscribe(GameEventType.WebGlVersion, OnWebGlVersion);
        }
        private void OnDisable()
        {
            GameEventBus.UnSubscribe(GameEventType.arcademode, OnSignInIIRCade);
#if !UNITY_WEBGL
            GameEventBus.UnSubscribe(GameEventType.PolyCadebuild, OnSignInPolyCade);
          //  GameEventBus.UnSubscribe(GameEventType.steamBuild, OnSteamSignIn);
#endif
            GameEventBus.UnSubscribe(GameEventType.WebGlVersion, OnWebGlVersion);
        }

        private void OnWebGlVersion()
        {


            string randUsername = "bowler" + Random.Range(1000, 5000).ToString();
            foreach (GameObject mode in Modes)
            {
                mode.SetActive(false);
            }
            Modes[0].SetActive(true);
            Modes[3].SetActive(true);
            MainMenuAndNetworkManager.UnlouchAchivment("isaiah", 0);
            DataBaseManager.Instance.RegisterLocallUser(randUsername);//register new user
                                                                      //        RandEmailSignit(randUsername, "iiRcade"); */
        }
        private void OnSignInIIRCade()
        {
            bashPanel.SetActive(false);
            string randUsername = "bowler" + Random.Range(1000, 5000).ToString();

            DataBaseManager.Instance.RegisterLocallUser(randUsername);//register new user
                                                                      //        RandEmailSignit(randUsername, "iiRcade"); */

        }
#if !UNITY_WEBGL
        private void OnSignInPolyCade()
        {
            string randUsername = "bowler" + Random.Range(1000, 5000).ToString();
            RandEmailSignit(randUsername, "polycade");

        }


        private void OnSteamSignIn()
        {


            steamApply.SetActive(true);

            if (!SteamManager.Initialized) return;
            string name = SteamFriends.GetPersonaName();

            RandEmailSignit(name, "steam");
        }



        private void RandEmailSignit(string username, string sign)
        {
            if (DataBaseManager.Instance != null && DataBaseManager.UserName != "" && DataBaseManager.playerData != null) return;
            if (PlayerPrefs.HasKey("email") && PlayerPrefs.HasKey("password"))
            {
                //Auto Login
                Debug.LogError("Auto Login ..");
                Login(PlayerPrefs.GetString("email"), PlayerPrefs.GetString("password"));
            }
            else
            {
                //No saved
                rememberMe.isOn = true;

                string randUsername = username;//"bowler" + Random.Range(1000, 5000).ToString();
                string randEmail = randUsername + "@" + sign  + ".cab";
                string randPassword = "@" + sign + sign;
                SignUp(randUsername, randEmail, randPassword, randPassword);
                Debug.LogError("Email = " + randEmail);
                Debug.LogError("Rand pass = " + randPassword);
            }
        }
#endif
#endregion

        private void Awake()
        {
            //auth = FirebaseAuth.DefaultInstance;
        }
        private void Start()
        {
           //if(PlayerPrefs.HasKey("email") && PlayerPrefs.HasKey("password")){
           //     //Auto Login
           //     Debug.Log("Auto Login ..");
           //     Login(PlayerPrefs.GetString("email"),PlayerPrefs.GetString("password"));
           // }
        }
#if !UNITY_WEBGL
        public void Login(string email,string password)
        {
            if (autAction) return;
            ClearErrorMessage();
            autAction = true;

            OnSignInEmailAndPass(email, password, (result1,result2) => 
            {

                if (result1 == "success") {

                    DataBaseManager.Instance.CheckUserName(result2.User.UserId);
                    //  autAction = false;
                    if (rememberMe.isOn)
                    {
                        //remember me
                        PlayerPrefs.SetString("email", email);
                        PlayerPrefs.SetString("password", password);
                    }
                }
                else
                {
                    ErrorMessage(1, result1);
                }

            });
            //auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(x =>
            //{
               
            //if (x.IsFaulted)
            //    {

            //        Debug.Log(x.Exception.InnerExceptions[0].Message);
            //        switch (x.Exception.InnerExceptions[0].Message)
            //        {
            //            case "The email address is badly formatted.":
            //                Debug.Log("first case test 1 ");
            //                 ErrorMessage(1,x.Exception.InnerExceptions[0].Message);
            //                break;
 
            //            default:
            //                ErrorMessage(1,"Please Check Your Password OR Email !!");
            //                Debug.Log("default case");
            //                break;
            //        }
                  
            //       return;
            //    }
            
            //    if (x.IsCompleted)
            //    {
            //        //login success
            //        Debug.Log("Login Success");

            //        DataBaseManager.Instance.CheckUserName(x.Result.User.UserId);
            //      //  autAction = false;
            //        if (rememberMe.isOn)
            //        {
            //            //remember me
            //            PlayerPrefs.SetString("email", email);
            //            PlayerPrefs.SetString("password", password);
            //        }
            //    }

            //});
      

        }
     
        public void SignUp(string username,string email,string password,string confirmpass)
        {
            if (autAction) return;
            if(username==""  || email == "")
            {
                ErrorMessage(0, "Please  Fill The Empty Fields !!");
                return;
            }
            if (password != confirmpass)
            {
                ErrorMessage(0, "Passwords Not Identical !!");
                return;
            }
            if(password.Length < 8)
            {
                ErrorMessage(0, "Your Password is Too Week !");
                return;
            }
            ClearErrorMessage();
             autAction = true;

            OnSignUpEmailAndPass(email, password, (result1,result2) =>
            {

                if(result1 == "success" && result2 !=null)
                {
                    Debug.Log("SignUp Success");
                    UiManager.Instance.ClearSignData();
                    StartCoroutine(DataBaseManager.Instance.RegisterNewUser(result2.User.UserId, username));
                    SuccessMessage("SignUp Succes !!");
                    if (rememberMe.isOn)
                    {
                        //remember me
                        PlayerPrefs.SetString("email", email);
                        PlayerPrefs.SetString("password", password);
                    }
                    autAction = false;

                }
                else
                {
                    ErrorMessage(0, result1);
                }
            });
            //auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(x =>
            //{
            //    //CheckIfEmailRegistered(auth, email);
            //    if (x.IsFaulted)
            //    {

            //        Debug.Log(x.Exception.InnerExceptions[0].Message);
            //        switch (x.Exception.InnerExceptions[0].Message)
            //        {
            //            case "The email address is badly formatted.":
            //                Debug.Log("first case test 1 ");
            //                ErrorMessage(0, x.Exception.InnerExceptions[0].Message);
            //                break;
            //            case "The email address is already in use by another account.":
            //                Debug.Log("first case test 1 ");
            //                 ErrorMessage(0,x.Exception.InnerExceptions[0].Message);
            //                break;
            //            case "The given password is invalid.":
            //                ErrorMessage(0, "Your Password is Too Week !");
            //                break;
            //            default:
            //                ErrorMessage(0,"Please Check Your Data !!");
            //                Debug.Log("default case");
            //                break;
            //        }
            //        return;
            //    }

            //    if (x.IsCompleted)
            //    {
            //        //login success
            //        Debug.Log("SignUp Success");
            //        UiManager.Instance.ClearSignData();
            //        StartCoroutine(DataBaseManager.Instance.RegisterNewUser(x.Result.User.UserId, username));
            //        SuccessMessage("SignUp Succes !!");
            //        if (rememberMe.isOn)
            //        {
            //            //remember me
            //            PlayerPrefs.SetString("email", email);
            //            PlayerPrefs.SetString("password", password);
            //        }
            //        autAction = false;
            //    }
            //});
        }
#endif
     
        public void ErrorMessage(int errormessageindex,string message)
        {
            errormessages[errormessageindex].text = message;
          
            autAction = false;


        }
        public void SuccessMessage(string message)
        {
            successMessage.text = message;
        }
        private void ClearErrorMessage()
        {
            foreach(TextMeshProUGUI errors in errormessages)
            {
                errors.text = string.Empty;
            }
        }

      
    
    }

}