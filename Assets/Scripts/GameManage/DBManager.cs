using Firebase.Database;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class DBManager : MonoBehaviour
{
    private  DatabaseReference DbReference;
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
            Debug.Log("not exist");
            //userNotexist
        }
        else if (taskCheckuSER.IsCompleted)
        {
            DataSnapshot snapshot = taskCheckuSER.Result;
            if (snapshot.Exists)//!= null)
            {
                Debug.Log("user exist");
               
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


        }

    }
    public IEnumerator GetPlayerMainData(string username,string childn)
    {
        var taskfetchdata = DbReference.Child("Users").Child(username).Child(childn).GetValueAsync();
        yield return new WaitUntil(predicate: () => taskfetchdata.IsCompleted);

        if (taskfetchdata.Exception != null)
        {


            Debug.Log(message: $"Failed to register task with {taskfetchdata.Exception}");

        }
        else
        {
            Debug.Log("completed");
            // PlayerMetaData playermeta = SavingMetaData.ReadFromJSON<PlayerMetaData>("GameMetaData");
            DataSnapshot snap = taskfetchdata.Result;
            Debug.Log(snap);

        }
    }
    public IEnumerator SetPlayerData(string username, string childn,string value)
    {
        var taskfetchdata = DbReference.Child("Users").Child(username).Child(childn).SetValueAsync(value);
        yield return new WaitUntil(predicate: () => taskfetchdata.IsCompleted);

        if (taskfetchdata.Exception != null)
        {


            Debug.Log(message: $"Failed to register task with {taskfetchdata.Exception}");

        }
        else
        {
            Debug.Log("completed");


        }
    }
    public IEnumerator CheckIfTokenExist(string tokenId)
    {
        var taskCheckuSER = DbReference.Child("tokens").Child(tokenId).GetValueAsync();
        yield return new WaitUntil(predicate: () => taskCheckuSER.IsCompleted);
        if (taskCheckuSER.IsFaulted)
        {
            Debug.LogError(taskCheckuSER.Result.ToString() + "faulted");
            Debug.Log(" token not exist");
            //userNotexist
        }
        else if (taskCheckuSER.IsCompleted)
        {
            DataSnapshot snapshot = taskCheckuSER.Result;
            if (snapshot.Exists)//!= null)
            {
                Debug.Log("token exist");
                StartCoroutine(CheckIfTokenExist(GenerateRandomTokent()));
            }
            else
            {
                Debug.Log(" token not exist");

                StartCoroutine(SetPlayerData(PhotonNetwork.LocalPlayer.NickName, tokenId, tokenId));
                StartCoroutine(SetTokenData(tokenId));
                //token not exist

            }
        }
        else if (taskCheckuSER.IsCanceled)
        {
            Debug.Log("not exist");
            //userNotexist
        }

    }
    public IEnumerator SetTokenData(string tokenId )
    {
        var taskfetchdata = DbReference.Child("tokens").Child(tokenId).SetValueAsync(tokenId);
        yield return new WaitUntil(predicate: () => taskfetchdata.IsCompleted);

        if (taskfetchdata.Exception != null)
        {


            Debug.Log(message: $"Failed to register task with {taskfetchdata.Exception}");

        }
        else
        {
            Debug.Log("completed");


        }
    }
    public static string GenerateRandomTokent()
    {
        string characters = "abcdefghigklmnobqrsxyuz1234567890";
        string randString = "";
        for(int i = 0; i < 8; i++)
        {
            randString += characters[Random.Range(0,characters.Length)];
            
        }
      return randString;
    }
}
