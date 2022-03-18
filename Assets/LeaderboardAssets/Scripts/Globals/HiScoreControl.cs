//----------------------------------------
//       HiScore Controller
//
//      Last Modified : 2021.03.08 
//       seungje.park@iircade.com
//----------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
[Serializable]
public class GlobalScoreRec {
    public string date;
    public string country;
    public string user;
    public int rankedpoints;
}

public class HiScoreControl : MonoBehaviour {

    [SerializeField] RectTransform content;       // ScrollView - content
    [SerializeField] GameObject txtError;         // network error message
   // [SerializeField] Button btnLocal;             // Local/Global button

    //[SerializeField] GameObject[] grideTitle;     // Local or Global  
 //   [SerializeField] Sprite[] spriteOnOff;        // Sprites of btnLocal  
    [SerializeField] private GameObject GloballScoreItem;

    bool isLocal = true;                          // current is local?  
    bool isNetworkError;                          // occurs network error to get global leaderboard from server?  

    // Global Leaderboard score list
    List<GlobalScoreRec> mGlobalScore = new List<GlobalScoreRec>();

    // for display score list
    string[] strDiff = { "Easy", "Medium", "Hard" };
    string[] strMode = { "Home", "Challenge" };

    //----------------------------
    //  OnEnable
    //----------------------------
    void OnEnable() {
        if (mGlobalScore.Count == 0) {
            StartCoroutine(DownloadGlobalScore());
           // SetLocalScore();
        }
    }
    void Start(){
     
        StartCoroutine(SetGlobal());
    }
    IEnumerator SetGlobal(){

        yield return new WaitForSeconds(1f);
        SetGlobalScore();
    }

    void SetGlobalScore() {

        ClearContents();                        // clear UI score elements

        int rank = 0;
        int count = 0;
        foreach (GlobalScoreRec score in mGlobalScore) {
            if(count <=9){//9){
            GameObject item = Instantiate(GloballScoreItem);
            item.name = "Score";
            rank ++;
           // item.transform.Find("Image").GetComponent<Image>().color        = color[rank++ % 2];
            item.transform.Find("TxtRank").GetComponent<Text>().text        = rank.ToString();
            item.transform.Find("TxtDate").GetComponent<Text>().text        = score.date;
            item.transform.Find("TxtCountry").GetComponent<Text>().text     = score.country;
            item.transform.Find("TxtName").GetComponent<Text>().text        = score.user;
            item.transform.Find("RankedPoints").GetComponent<Text>().text = score.rankedpoints.ToString("#,0");
                
            item.transform.SetParent(content.transform, false);
            count++;
            }
        }
    }
   
    //----------------------------
    //  Download Global Score <- OnEnable
    //----------------------------
    IEnumerator DownloadGlobalScore() {

        // Form Data Settings
        WWWForm form = new WWWForm();
        form.AddField("request", "load");
        form.AddField("game", Globals.game_id);
       
        using (UnityWebRequest request = UnityWebRequest.Post(Globals.serverURL, form)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError) {
                isNetworkError = true;
                yield break;
            }

            MakeGlobalScoreList(request.downloadHandler.text);
        }
    }

    //----------------------------
    //  Make Global Score <- DownloadGlobalScore
    //----------------------------
    private void MakeGlobalScoreList(string text) {

        // Always check received data.
        // You may get wrong data for your wrong request or other reasons.
        // At that time, an error message is sent to this text.
        Debug.Log(text);

        mGlobalScore.Clear();
        if (text.Trim().Equals("")) return;

        // break text one by one - buffer is single user data 
        string[] buffer = text.Trim().Split('\n');

        try {
            // break string to each elements
            foreach (string t in buffer) {
                GlobalScoreRec score = new GlobalScoreRec();
                string[] s = t.Split('|');

                // follow server data sequence
                score.date       = s[0];
                score.country    = s[1];
                score.user       = s[2];
              //  score.rankedpoints = int.Parse(s[3]);
                score.rankedpoints = int.Parse(s[4]);
                
                mGlobalScore.Add(score);
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }
    //----------------------------
    //  Clear Contents <- SetLocalScore
    //----------------------------
    void ClearContents() {
        for (int i = content.childCount - 1; i >= 0; i--) {
            if(content.GetChild(i).gameObject.name != "BackButt" && content.GetChild(i).gameObject.name != "Type"){
            Destroy(content.GetChild(i).gameObject);
            }
        }
    }

}
