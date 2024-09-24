using Firebase.Auth;
using Firebase.Extensions;
using System;
using UnityEngine;
using Google;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using TMPro;

namespace BackEnd
{

    public class AuthManager : Singelton<AuthManager>
    {
        private FirebaseAuth auth;

        [SerializeField] protected bool autAction;
        [SerializeField] BashAuth bashAuth;
        public TextMeshProUGUI debugTxt;
        public BashAuth BashAuth { get { return bashAuth; } }
        public bool AuthAction { get { return autAction; } set { autAction = value; } }

        #region Mobile
        private GoogleSignInConfiguration configuration;
        [SerializeField] private string webclientId;
        [SerializeField] private CanvasGroup mobileGroup;
        [SerializeField] private GameObject waitingPanel;
        #endregion


        private void OnEnable()
        {
            configuration = new GoogleSignInConfiguration { WebClientId = webclientId, RequestEmail = true, RequestIdToken = true };
        }

        [ContextMenu("Test auth data local save")]
        private void Test()
        {
            PlayerPrefs.SetString("userid", "qweoqoweoqeoqwoeoqw");
            PlayerPrefs.SetString("username", "Fares bakr");
        }

        private void Start()
        {
            //check if already local data sign in
            if (PlayerPrefs.HasKey("userid") && PlayerPrefs.HasKey("username"))
            {
                //
                mobileGroup.interactable = false;
                waitingPanel.gameObject.SetActive(true);
                DataBaseManager.SignIn?.Invoke(PlayerPrefs.GetString("userid"), true, PlayerPrefs.GetString("username"));//load local data
            }
        }
        public virtual void OnSignInGoogle()
        {
            OnSignIn();
        }
        public void OnSignInApple()
        {
            mobileGroup.interactable = false;
         
            Firebase.Auth.FederatedOAuthProviderData providerData =
            new Firebase.Auth.FederatedOAuthProviderData();
            providerData.ProviderId = "apple.com";

            Firebase.Auth.FederatedOAuthProvider provider =
            new Firebase.Auth.FederatedOAuthProvider();
            provider.SetProviderData(providerData);

            if (auth == null) auth = FirebaseAuth.DefaultInstance;
            auth.SignInWithProviderAsync(provider).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    // Debug.LogError("SignInWithProviderAsync was canceled.");
                    mobileGroup.interactable = true;
                    return;
                }
                if (task.IsFaulted)
                {
                    mobileGroup.interactable = true;
                    debugTxt.text = "SignInWithProviderAsync encountered an error: " +
                      task.Exception;
                    return;
                }

                Firebase.Auth.AuthResult authResult = task.Result;
                Firebase.Auth.FirebaseUser user = authResult.User;
                //sucess save data
                OnSaveSignData(authResult.User.UserId,authResult.User.DisplayName);
                DataBaseManager.SignIn?.Invoke(authResult.User.UserId, true, authResult.User.DisplayName);
                debugTxt.text = "Success";

                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    user.DisplayName, user.UserId);
            });
        }
        #region Google
        private void OnSignIn()
        {
            mobileGroup.interactable = false;
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }


        internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsFaulted)
            {
                using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                        mobileGroup.interactable = true;
                        debugTxt.text = "\nError code = " + error + " Message = ";
                    }
                    else
                    {
                        mobileGroup.interactable = true;
                    }
                }
            }
            else if (task.IsCanceled)
            {
                mobileGroup.interactable = true;
            }
            else
            {


                debugTxt.text = task.Result.IdToken;
                SignInWithGoogleOnFirebase(task.Result.IdToken);
            }
        }
        private void SignInWithGoogleOnFirebase(string idToken)
        {
            Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
            if (auth == null) auth = FirebaseAuth.DefaultInstance;
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                AggregateException ex = task.Exception;
                if (ex != null)
                {
                    if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    {
                        debugTxt.text += "\nError code = " + inner.ErrorCode + " Message = " + inner.Message;
                        mobileGroup.interactable = true;
                    }
                      //  AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
                }
                else
                {
                   
                    OnSaveSignData(task.Result.UserId, task.Result.DisplayName);
                    DataBaseManager.SignIn?.Invoke(task.Result.UserId,true,task.Result.DisplayName);
                    debugTxt.text = "Success";

                    //  User = task.Result;

                    //  StartCoroutine(LoadUserData());

                }
            });
        }

        #endregion
        protected virtual void OnSignUpEmailAndPass(string email,string password,Action<string,AuthResult> callback)
        {
           if(auth == null) auth = FirebaseAuth.DefaultInstance;
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(x =>
            {
                //CheckIfEmailRegistered(auth, email);
                if (x.IsFaulted)
                {

                    Debug.Log(x.Exception.InnerExceptions[0].Message);
                    switch (x.Exception.InnerExceptions[0].Message)
                    {
                        case "The email address is badly formatted.":
                            Debug.Log("first case test 1 ");
                            callback?.Invoke(x.Exception.InnerExceptions[0].Message, null);
                        
                            break;
                        case "The email address is already in use by another account.":
                            Debug.Log("first case test 1 ");
                            callback?.Invoke(x.Exception.InnerExceptions[0].Message,null);
                            break;
                        case "The given password is invalid.":
                            callback?.Invoke("Your Password is Too Week !",null);
                          
                            break;
                        default:
                            callback?.Invoke("Please Check Your Data !!",null);
                         
                            Debug.Log("default case");
                            break;
                    }
                    return;
                }

                if (x.IsCompleted)
                {
                    //login success
                    callback?.Invoke("success",x.Result);
                    Debug.Log("SignUp Success");
                 
                }
            });
        }
        protected virtual void OnSignInEmailAndPass(string email, string password,Action<string,AuthResult> callBack)
        {
            auth = FirebaseAuth.DefaultInstance;
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(x =>
            {

                if (x.IsFaulted)
                {

                    Debug.Log(x.Exception.InnerExceptions[0].Message);
                    switch (x.Exception.InnerExceptions[0].Message)
                    {
                        case "The email address is badly formatted.":
                            Debug.Log("first case test 1 ");
                            callBack?.Invoke(x.Exception.InnerExceptions[0].Message,null);
                        //    ErrorMessage(1, x.Exception.InnerExceptions[0].Message);
                            break;

                        default:
                            callBack?.Invoke("Please Check Your Password OR Email !!",null);
                          //  ErrorMessage(1, "Please Check Your Password OR Email !!");
                            Debug.Log("default case");
                            break;
                    }

                    return;
                }

                if (x.IsCompleted)
                {
                    //login success
                    Debug.Log("Login Success");
                    callBack?.Invoke("success", x.Result);
                 
                    //DataBaseManager.Instance.CheckUserName(x.Result.User.UserId);
                    ////  autAction = false;
                    //if (rememberMe.isOn)
                    //{
                    //    //remember me
                    //    PlayerPrefs.SetString("email", email);
                    //    PlayerPrefs.SetString("password", password);
                    //}
                }

            });
        }

        private void OnSaveSignData(string uid,string username)
        {
            if (PlayerPrefs.HasKey("userid") || PlayerPrefs.HasKey("username")) return;
            
            PlayerPrefs.SetString("userid", uid);
            PlayerPrefs.SetString("username", username);
            PlayerPrefs.Save();
        }
    }

}