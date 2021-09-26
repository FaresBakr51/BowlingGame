//----------------------------------------
//      Game Over or Misson Complete
//
//      Last Modified : 2021.03.08 
//       seungje.park@iircade.com
//----------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameOverManager : MonoBehaviour {

    // UI widgets
    [SerializeField] Text txtScore;
    [SerializeField] Text txtLive;
    [SerializeField] Text txtInput;
    [SerializeField] Button btnBack;

    // VirtualKeyboad script
    public VirtualKeyboad keyboard;     

    // for save player score to current device
    LocalScoreRec mScore = new LocalScoreRec();

    // [Back] button animator
    Animator anim;

    //------------------------------
    // Start 
    //------------------------------
    void Start() {
        // regist VirtualKeyboad's OnKeyPressed event
        keyboard.OnKeyPressed += OnKeyPressed;

        keyboard.InputText = "";

        // aimater had 'Open' and 'Close' animation
        anim = btnBack.GetComponent<Animator>();

        // make dummy score
        MakeScore();
    }

    //------------------------------
    // Make Dummy Score
    //------------------------------
    private void MakeScore() {
        mScore.date         = System.DateTime.Now.ToString("yyyy-MM-dd");
        mScore.score        = Random.Range(10000, 900000);
        mScore.lives        = Globals.Options.lives;
        mScore.difficulty   = Globals.Options.difficulty;
        mScore.mode         = Globals.Options.mode;

        txtScore.text = mScore.score.ToString();
    }

    //-----------------------------------
    // Set Focus <- calls other scripts
    //-----------------------------------
    public void SetFocus(Button button) {
        StartCoroutine(ButtonSelect(button));
    }

    //--------------------------------------
    // Button Select - focus any button
    //--------------------------------------
    IEnumerator ButtonSelect(Button button) {
        // delay a little bit time - Unity bug?
        yield return new WaitForSeconds(0.02f);
        button.Select();
    }

    //-----------------------------------
    // Save Score <- OnButtonClick
    //-----------------------------------
    void SaveScore() {
        mScore.user = txtInput.text;    // set user name
        Globals.SaveScore(mScore);      // save local score to current device
        StartCoroutine(PostScore());    // Send score to Global leader board(Ranking server)
    }

    //-----------------------------------
    // Post Server <- SaveScore
    //-----------------------------------
    IEnumerator PostScore() {
        // form data settings
        // field order doesn't matter, but field names must be correct

        WWWForm form = new WWWForm();
        form.AddField("request",    "save");                // must use 'save' - lowercase
        form.AddField("game",       Globals.game_id);       // game id
        form.AddField("user",       mScore.user);           // user name
        form.AddField("score",      mScore.score);          // game score
        form.AddField("lives",      mScore.lives);          // lives
        form.AddField("difficulty", mScore.difficulty);     // difficulty
        form.AddField("stage",      mScore.mode);           // Server has no mode field, so we use stage field instead
        form.AddField("mac",        Globals.GetMacAddr());  // device MAC address

        using (UnityWebRequest request = UnityWebRequest.Post(Globals.serverURL, form)) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log("Network Error");
            }
        }

        SceneManager.LoadSceneAsync("MainMenu");
    }

    //------------------------------
    // On Disable
    //------------------------------
    void OnDisable() {
        // unregist keyboard event
        keyboard.OnKeyPressed -= OnKeyPressed;
    }

    //------------------------------
    // OnKeyPressed <- Virtual keyboard event  
    //------------------------------
    void OnKeyPressed(string key) {
        txtInput.text = keyboard.InputText;

        if (key.Equals("Enter")) {
            anim.Play("Open", -1, 0);   // move [Back] button inside of scene
            SetFocus(btnBack);          
        }
    }

    //-------------------------------------------------------
    // OnInput Selected
    //-------------------------------------------------------
    public void OnInputSelected() {
        // when the [Back] button is in focus, pressing the up arrow key or moving the joystick up invokes this event
        anim.Play("Close", -1, 0);      // move [Back] button outside of scene
        keyboard.OpenKeyboard();        // move [Keyboard] inside of scene
    }

    //----------------------------------------
    // OnButtonClick <- Buttons Submit Event
    //----------------------------------------
    public void OnButtonClick(Button button) {
        switch (button.name) {
        case "BtnBack":
            SaveScore();
            break;
        }
    }
}
