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

[Serializable]
public class GlobalScoreRec {
    public string date;
    public string country;
    public string user;
    public int    score;
    public int    lives;
    public int    difficulty;
    public int    mode;
}

public class HiScoreControl : MonoBehaviour {

    [SerializeField] RectTransform content;       // ScrollView - content
    [SerializeField] GameObject txtError;         // network error message
    [SerializeField] Button btnLocal;             // Local/Global button

    [SerializeField] GameObject[] grideTitle;     // Local or Global  
    [SerializeField] Sprite[] spriteOnOff;        // Sprites of btnLocal  

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
            SetLocalScore();
        }
    }

    //----------------------------
    //  Set Local Score <- OnEnable / OnButtonSubmit
    //----------------------------
    private void SetLocalScore() {

        Globals.LoadScore();            // load local score from current device
        ClearContents();                // clear UI score elements
        txtError.SetActive(false);      // local score can not occurs network error

        // alternately repeat item's background color
        Color[] color = new Color[] { new Color(0, 0, 0, 0.4f), new Color(0.2f, 0.2f, 0.2f, 0.4f) };

        for (int i = 0; i < Globals.mLocalScore.Count; i++) {
            // get each element's parent - see 'Resources/Global(Local)ScoreItem' object
            GameObject item = Instantiate(Resources.Load("LocalScoreItem")) as GameObject;
            item.name = "Score";

            // set score items to parent
            item.transform.Find("Image").GetComponent<Image>().color       = color[i % 2];
            item.transform.Find("TxtRank").GetComponent<Text>().text       = (i + 1).ToString();
            item.transform.Find("TxtDate").GetComponent<Text>().text       = Globals.mLocalScore[i].date; 
            item.transform.Find("TxtName").GetComponent<Text>().text       = Globals.mLocalScore[i].user;
            item.transform.Find("TxtLives").GetComponent<Text>().text      = Globals.mLocalScore[i].lives.ToString("0");

            // translate numeric data to string
            item.transform.Find("TxtDifficulty").GetComponent<Text>().text = strDiff[Globals.mLocalScore[i].difficulty];
            item.transform.Find("TxtMode").GetComponent<Text>().text       = strMode[Globals.mLocalScore[i].mode];

            item.transform.Find("TxtScore").GetComponent<Text>().text      = Globals.mLocalScore[i].score.ToString("#,0");
            item.transform.SetParent(content.transform, false);
        }
    }

    //----------------------------
    //  Set Global Score <- OnButtonSubmit
    //----------------------------
    void SetGlobalScore() {

        ClearContents();                        // clear UI score elements
        txtError.SetActive(isNetworkError);     // display network error message

        // alternately repeat item's background color
        Color[] color = new Color[] { new Color(0, 0, 0, 0.4f), new Color(0.2f, 0.2f, 0.2f, 0.4f) };

        // when occurs network error, mGlobalScore is null, so skip after these statements
        int rank = 0;
        foreach (GlobalScoreRec score in mGlobalScore) {
            GameObject item = Instantiate(Resources.Load("GloballScoreItem")) as GameObject;
            item.name = "Score";

            item.transform.Find("Image").GetComponent<Image>().color        = color[rank++ % 2];
            item.transform.Find("TxtRank").GetComponent<Text>().text        = rank.ToString();
            item.transform.Find("TxtDate").GetComponent<Text>().text        = score.date;
            item.transform.Find("TxtCountry").GetComponent<Text>().text     = score.country;
            item.transform.Find("TxtName").GetComponent<Text>().text        = score.user;
            item.transform.Find("TxtLives").GetComponent<Text>().text       = score.lives.ToString("0");

            // translate numeric data to string
            item.transform.Find("TxtDifficulty").GetComponent<Text>().text  = strDiff[score.difficulty]; 
            item.transform.Find("TxtMode").GetComponent<Text>().text        = strMode[score.mode]; 
            item.transform.Find("TxtScore").GetComponent<Text>().text       = score.score.ToString("#,0");

            item.transform.SetParent(content.transform, false);
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
                score.lives      = int.Parse(s[3]);
                score.score      = int.Parse(s[4]);
                score.difficulty = int.Parse(s[5]);
                score.mode       = int.Parse(s[6]);     // server field name is 'stage'
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
            Destroy(content.GetChild(i).gameObject);
        }
    }

    //----------------------------
    //  Local/Global Button Submit
    //----------------------------
    public void OnButtonSubmit() {
        // toggle local global
        isLocal = !isLocal;

        // set contents title
        grideTitle[0].SetActive(isLocal);
        grideTitle[1].SetActive(!isLocal);

        // set button sprite
        btnLocal.image.sprite = isLocal ? spriteOnOff[0] : spriteOnOff[1];

        if (isLocal)
            SetLocalScore();
        else
            SetGlobalScore();
    }
}
