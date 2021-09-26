//----------------------------------------
//     Global and static variables
//
//      Last Modified : 2021.03.08 
//       seungje.park@iircade.com
//----------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;
using System.Linq;

[Serializable]
public class OptionRec {
    public bool canMusic   = true;
    public bool canSFX     = true;
    public bool canJump    = true;
    public bool canRun     = true;
    public int  lives      = 3;
    public int  difficulty = 0;        // easy, medium, hard, etc
    public int  mode       = 0;        // home, challenge, etc
}

[Serializable]
public class LocalScoreRec {
    public string date;             // play date                        
    public string user;             // player name
    public int    score;            // score
    public int    lives;            // by option setting
    public int    difficulty;       // by option setting 
    public int    mode;             // by option setting 
}

public class Globals : MonoBehaviour {

    // option & local score File Path
    static string optionsFile = Application.persistentDataPath + "/Options.dat";
    static string scoreFile   = Application.persistentDataPath + "/Score.dat";

    // global leaderboard url & your game id
    public static string serverURL = "http://score.iircade.com/ranking/ranking_test.php";
    public static string game_id   = "MR_1";

    // options
    public static OptionRec Options = new OptionRec();

    // local score List
    public static List<LocalScoreRec>  mLocalScore  = new List<LocalScoreRec>();

    //----------------------- public functions ----------------------------------

    //-----------------------------------
    // Load Options <- Options Menu
    //-----------------------------------
    public static void LoadOptions() {
        if (!File.Exists(optionsFile)) return;

        using (Stream stream = File.Open(optionsFile, FileMode.Open)) {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Options = (OptionRec)binaryFormatter.Deserialize(stream);
            stream.Close();
        }
    }

    //-----------------------------------
    // Save Options <- Options Menu
    //-----------------------------------
    public static void SaveOptions() {
        using (Stream stream = File.Open(optionsFile, FileMode.Create)) {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, Options);
            stream.Close();
        }
    }

    //-----------------------------------
    // Load Local Score <- High Score Menu
    //-----------------------------------
    public static void LoadScore() {
        if (!File.Exists(scoreFile)) return;

        using (Stream stream = File.Open(scoreFile, FileMode.Open)) {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            mLocalScore = (List<LocalScoreRec>)binaryFormatter.Deserialize(stream);
            stream.Close();
        }
    }

    //-----------------------------------
    // Save Local Score <- Game Over
    //-----------------------------------
    public static void SaveScore(LocalScoreRec mScore) {
        // find player minimum score
        LoadScore();
        mLocalScore = mLocalScore.FindAll(x => x.user.Equals(mScore.user)).OrderBy(x => x.score).ToList();

        // below own minimum score?
        if (mLocalScore.Count > 0 && mLocalScore[0].score >= mScore.score) return;

        // append player score
        LoadScore();
        mLocalScore.Add(mScore);

        // descending sort
        mLocalScore = mLocalScore.OrderByDescending(x => x.score).ToList();

        // save records under 30 elements
        if (mLocalScore.Count > 30) {
            mLocalScore.RemoveAt(30);
        }

        // Save records
        using (Stream stream = File.Open(scoreFile, FileMode.Create)) {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            binaryFormatter.Serialize(stream, mLocalScore);
            stream.Close();
        }
    }

    //-----------------------------------
    // Get MAC Address <- Game Over
    //-----------------------------------
    public static string GetMacAddr() {
        string macAdress = "";
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface adapter in nics) {
            PhysicalAddress address = adapter.GetPhysicalAddress();
            if (address.ToString() != "") {

                // mac address is 12 characters, use from 2 with 10 length characters
                macAdress = address.ToString().Substring(2, 10);
                return macAdress;
            }
        }

        return macAdress;
    }
}
