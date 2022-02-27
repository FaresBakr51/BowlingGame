using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    [SerializeField] Button btnStart;

    // Start is called before the first frame update
    void Start() {
        // for Android device
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // must one button has focus 
        btnStart.Select();
    }

    // Update is called once per frame
    void Update() {

    }

    // button submit event
    public void OnButtonSubmit() {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
