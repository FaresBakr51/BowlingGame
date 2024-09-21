using Firebase.Auth;
using Firebase.Extensions;
using System;
using UnityEngine;

namespace BackEnd
{

    public class AuthManager : Singelton<AuthManager>
    {
        private FirebaseAuth auth;

        [SerializeField] protected bool autAction;
        public bool AuthAction { get { return autAction; } set { autAction = value; } }

        #region Mobile
        private GoogleSignInConfiguration configuration;
        #endregion
        public virtual void OnSignInGoogle()
        {

        }
        public virtual void OnSignInEmailAndPass(string email, string password,Action<string,AuthResult> callBack)
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
    }

}