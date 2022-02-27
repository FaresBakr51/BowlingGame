//--------------------------------------
//             VirtualKeyboad
//
//      Last Modified : 2021.03.08 
//       seungje.park@iircade.com
//--------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// iiRcade Game Console, PS4 Controller buttons definition
public enum VKEY {
    Btn_A = 350,        // Left button set
    Btn_B = 351,
    Btn_C = 352,
    Btn_X = 353,
    Btn_Y = 354,
    Btn_Z = 355,

    Btn_A2 = 370,       // Right button set
    Btn_B2 = 371,
    Btn_C2 = 372,
    Btn_X2 = 373,
    Btn_Y2 = 374,
    Btn_Z2 = 375,

    Btn_Start1   = 360,   
    Btn_Coin     = 361,
    Btn_Start2   = 380,

    PS4_Square   = 350,     // PS4 Controller botton set
    PS4_X        = 351,
    PS4_O        = 352,
    PS4_Triangle = 353,

    PS4_LTriggerUp = 374,   // Ps4 Trigger up button
    PS4_RTriggerUp = 375
}

public class VirtualKeyboad : MonoBehaviour {

    // virtual key event delegate
    public delegate void OnKeypressEvent(string key);
    public event OnKeypressEvent OnKeyPressed;

    // virtual key input text - share to any scripts
    public string InputText = "";

    // alpha keys list
    List<Transform> alphaKeys = new List<Transform>();

    // current selected button 
    Button curBtn;

    string buff = "";
    bool isCapslock = true;
    Animator anim;

    //-------------------------
    // Start
    //-------------------------
    void Start() {
        // get all alphabet keys
        GetAlphaKeys();

        // animator has 'Open' and 'Close' animation
        anim = GetComponent<Animator>();

        // default focused button
        curBtn = alphaKeys.Find(x => x.name.Equals("T")).GetComponent<Button>();
        SetFocus(curBtn);

        // lower case default
        ToggleCapsLock();   
    }

    //-------------------------
    // Update
    //-------------------------
    void Update() {
        // any button press? (except [A])
        // this function is Game console's hot key(hot button)
        // [B] BackSpace, [C] Esc, [X] Caps lock, [Y] Space, [Z] Enter

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(vKey) && (int)vKey > (int)VKEY.Btn_A) {    
                SetHotKey((int)vKey);
                return;
            }
        }
    }

    //-----------------------------------
    // Set Input Key <- Update
    //-----------------------------------
    void SetHotKey(int vKey) {
        // Game console: [Start_1][Start_2][Coin], PS4 controller: [LStick], [RStick], [No match]
        if (vKey == (int)VKEY.Btn_Start1 || vKey == (int)VKEY.Btn_Start2 || vKey == (int)VKEY.Btn_Coin) return;

        switch (vKey) {
        case (int)VKEY.Btn_B:       // BS - Console [B], PS4 [X]
        case (int)VKEY.Btn_B2:
            SetBackSpace();
            break;
        case (int)VKEY.Btn_C:       // Esc - Console [C], PS4 [O]
        case (int)VKEY.Btn_C2:
            buff = "";
            break;
        case (int)VKEY.Btn_X:      // CapsLock - Console [X], PS4 [Triangle]
        case (int)VKEY.Btn_X2:
            ToggleCapsLock();
            break;
        case (int)VKEY.Btn_Y:      // Space - Console [Y], PS4 [Left Trigger_Up]
        case (int)VKEY.Btn_Y2:
            buff += " "; 
            break;
        case (int)VKEY.Btn_Z:      // Enter - Console [Z], PS4 [Right Trigger_Up]
        case (int)VKEY.Btn_Z2:
            CloseKeyboard();
            OnKeyPressed("Enter");
            return;
        }

        InputText = buff;
        OnKeyPressed("");
    }

    //-----------------------------------
    // Set BackSpace <- SetInputKey
    //-----------------------------------
    void SetBackSpace() {
        int n = buff.Length;
        buff = (n >= 1) ? buff.Substring(0, buff.Length - 1) : "";
    }

    //-----------------------------------
    // Clear text buffer
    //-----------------------------------
    public void Clear() {
        InputText = buff = "";
    }

    //-----------------------------------
    // Open Keyboard
    //-----------------------------------
    public void OpenKeyboard() {
        // move keyboard inside of active scene
        anim.Play("Open", -1, 0);

        // current key set focus(select)
        SetFocus(curBtn);
    }

    //-----------------------------------
    // Close Keyboard <- SetInputKey
    //-----------------------------------
    void CloseKeyboard() {
        // move keyboard outside of active scene
        anim.Play("Close", -1, 0);
    }

    //-----------------------------------
    // Caps lock On/Off <- SetInputKey
    //-----------------------------------
    void ToggleCapsLock() {
        isCapslock = !isCapslock;

        foreach (Transform key in alphaKeys) {
            Text keycap = key.GetComponentInChildren<Text>();
            keycap.text = isCapslock ? keycap.text.ToUpper() : keycap.text.ToLower();
        }
    }

    //-----------------------------------
    // Set Focus <- OpenKeyboard
    //-----------------------------------
    void SetFocus(Button button) {
        StartCoroutine(ButtonSelect(button));
    }

    //-----------------------------------
    // Button Select <- SetFocus
    //-----------------------------------
    IEnumerator ButtonSelect(Button button) {
        // delay a little bit time - Unity bug?
        yield return new WaitForSeconds(0.01f);
        button.Select();
    }

    //-------------------------
    // Get Alpha Keys <- Start
    //-------------------------
    void GetAlphaKeys() {
        alphaKeys.Clear();
        foreach (Transform line in transform) {     // find all child lines
            foreach (Transform key in line) {       // find all child keys in line
                if (key.name.Length == 1 && key.name.CompareTo("A") >= 0 && key.name.CompareTo("Z") <= 0) {
                    alphaKeys.Add(key);
                }
            }
        }
    }

    //---------------------------------------
    // On Button Click <- Key submit event
    //---------------------------------------
    public void OnButtonClick(Button button) {
        curBtn = button;
        switch (button.name) {
        case "CapsLock":
            ToggleCapsLock();
            break;
        case "Enter":
            CloseKeyboard();
            break;
        case "Esc":
            buff = "";
            break;
        case "Space":
            buff += " ";
            break;
        case "BS":
            SetBackSpace();
            break;
        default:
            buff += button.GetComponentInChildren<Text>().text; 
            break;
        }

        // maximun length is 30
        int len = (buff.Length >= 30) ? 30 : buff.Length;
        buff = buff.Substring(0, len);
        InputText = buff;

        // call other script's OnKeyPressed event
        OnKeyPressed(button.name);
        SetFocus(curBtn);
    }
}
