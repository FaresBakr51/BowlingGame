//-----------------------------------
//        Options Control
//
//      Last Modified : 2021.03.08 
//       seungje.park@iircade.com
//-----------------------------------

using UnityEngine;
using UnityEngine.UI;

public class OptionControl : MonoBehaviour {

    // Option Buttons
    [SerializeField] Button btnMusic;
    [SerializeField] Button btnSFX;
    [SerializeField] Button btnJump;
    [SerializeField] Button btnRun;
    [SerializeField] Button btnLives;
    [SerializeField] Button btnDifficulty;
    [SerializeField] Button btnMode;

    [SerializeField] Sprite imgOn;
    [SerializeField] Sprite imgOff;

    Button btnCurrent = null;       // current focused button
    float axisX = 0;                // stick value
    bool isAxisUse = false;         // can use joystick? - avoid automatic repetition

    string[] strDiff = { "Easy", "Medium", "Hard" };
    string[] strMode = { "Home", "Challenge" };

    //-----------------------------------
    // OnEnable
    //-----------------------------------
    void OnEnable() {
      //  Globals.LoadOptions();
        SetOptions();

        btnCurrent = btnMusic;
        btnCurrent.Select();
    }

    //-----------------------------------
    // Update
    //-----------------------------------
    void Update() {

        // check joystick or keyboard - keyboard use for PC testing
        axisX = Input.GetAxisRaw("DPAD_Hor") + Input.GetAxisRaw("Horizontal");
        if (axisX >= 0.9f || axisX <= -0.9f) {
            if (!isAxisUse && btnCurrent != null) {
                OnOptionButtonSubmit(btnCurrent);
                isAxisUse = true;
            }
        }

        // avoid automatic repetition
        if (axisX == 0) {
            isAxisUse = false;
        }
    }

    //-----------------------------------
    // OnDisable
    //-----------------------------------
 /*    void OnDisable() {
        // if close this panel, save options
        if (btnCurrent != null) Globals.SaveOptions();
    } */

    //-----------------------------------
    // Set Option by load option's data
    //-----------------------------------
    private void SetOptions() {
        btnMusic.image.sprite = Globals.Options.canMusic ? imgOn : imgOff;
        btnSFX.image.sprite   = Globals.Options.canSFX   ? imgOn : imgOff;
        btnJump.image.sprite  = Globals.Options.canJump  ? imgOn : imgOff;
        btnRun.image.sprite   = Globals.Options.canRun   ? imgOn : imgOff;

        // translate numeric data to string
        btnLives.transform.GetComponentInChildren<Text>().text      = Globals.Options.lives.ToString();
        btnDifficulty.transform.GetComponentInChildren<Text>().text = strDiff[Globals.Options.difficulty];
        btnMode.transform.GetComponentInChildren<Text>().text       = strMode[Globals.Options.mode];
    }

    //-----------------------------------
    // Set Music
    //-----------------------------------
    private void SetMusic() {
        Globals.Options.canMusic = !Globals.Options.canMusic;
        btnMusic.image.sprite = Globals.Options.canMusic ? imgOn : imgOff;
    }

    //-----------------------------------
    // Set SFX
    //-----------------------------------
    private void SetSFX() {
        Globals.Options.canSFX = !Globals.Options.canSFX;
        btnSFX.image.sprite = Globals.Options.canSFX ? imgOn : imgOff;
    }

    //-----------------------------------
    // Set Jump
    //-----------------------------------
    private void SetJump() {
        Globals.Options.canJump = !Globals.Options.canJump;
        btnJump.image.sprite = Globals.Options.canJump ? imgOn : imgOff;
    }

    //-----------------------------------
    // Set Run
    //-----------------------------------
    private void SetRun() {
        Globals.Options.canRun = !Globals.Options.canRun;
        btnRun.image.sprite = Globals.Options.canRun ? imgOn : imgOff;
    }

    //-----------------------------------
    // Set Lives
    //-----------------------------------
    private void SetLives() {
        Globals.Options.lives = 8 - Globals.Options.lives;
        btnLives.transform.GetComponentInChildren<Text>().text = Globals.Options.lives.ToString();
    }

    //-----------------------------------
    // Set Difficulty
    //-----------------------------------
    private void SetDifficulty() {

        // difficulty value is 0 ~ 2
        int n = Globals.Options.difficulty;

        // button click
        if (axisX == 0) {
            n = (n < 2) ? n + 1 : 0;
        }
        // stick movement
        else if (axisX > 0) {
             n = (n < 2) ? n + 1 : 0;
        } else if (axisX < 0) {
            n = (n > 0) ? n - 1 : 2;
        }

        Globals.Options.difficulty = n;
        btnDifficulty.transform.GetComponentInChildren<Text>().text = strDiff[n];
    }

    //-----------------------------------
    // Set Mode
    //-----------------------------------
    private void SetMode() {
        Globals.Options.mode = 1 - Globals.Options.mode;
        btnMode.transform.GetComponentInChildren<Text>().text = strMode[Globals.Options.mode];
    }

    //-----------------------------------
    // Button Selected
    //-----------------------------------
    public void OnOptionButtonSelect(Button button) {
        btnCurrent = button;
    }

    //---------------------------
    // On OptionButton Submit
    //---------------------------
    public void OnOptionButtonSubmit(Button button) {
        switch (button.name) {
        case "BtnMusic":
            SetMusic();
            break;
        case "BtnSFX":
            SetSFX();
            break;
        case "BtnJump":
            SetJump();
            break;
        case "BtnRun":
            SetRun();
            break;
        case "BtnLives":
            SetLives();
            break;
        case "BtnDifficulty":
            SetDifficulty();
            break;
        case "BtnMode":
            SetMode();
            break;
        }
    }
}
