//----------------------------------------
//        Control Main Menu
//
//      Last Modified : 2021.03.08 
//       seungje.park@iircade.com
//----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    // UI panel list
    [SerializeField] List<GameObject> mPanels;

    // for switch panels or popup panel
    Stack<GameObject> mStack = new Stack<GameObject>();

    // current panel & switch(popup) panel
    GameObject pnCurrent;
    GameObject pnNew;

    //-----------------------------------
    // Start
    //-----------------------------------
    void Start() {
        InitGame();
    }

    //-----------------------------------
    // Update
    //-----------------------------------
    void Update() {

    }

    //-----------------------------------
    // Open Panel
    //-----------------------------------
    void OpenPanel(string pnName) {
        // save current panel
        mStack.Push(pnCurrent);

        // select new panel
        pnNew = mPanels.Find(x => x.name.Equals(pnName)).gameObject;
        StartCoroutine(ChangePanels()); 
    }

    //-----------------------------------
    // Close Panel
    //-----------------------------------
    void ClosePanel() {
        if (mStack.Count == 0) return;

        // select saved panel
        pnNew = mStack.Pop();
        StartCoroutine(ChangePanels());
    }

    //-----------------------------------
    // Panel Fade-in/out
    //-----------------------------------
    IEnumerator ChangePanels() {
        pnNew.SetActive(true);

        // for cross fade
        CanvasGroup cg1 = pnCurrent.GetComponent<CanvasGroup>();
        CanvasGroup cg2 = pnNew.GetComponent<CanvasGroup>();

        float duration = 0.8f;      

        // set panel's alpha
        cg1.alpha = 1;
        cg2.alpha = 0;

        while (true) {
            // fadeout & fadein
            cg1.alpha -= Time.deltaTime / duration;
            cg2.alpha += Time.deltaTime / duration;

            if (cg2.alpha >= 1f) break;
            yield return null;
        }

        // switch panels
        pnCurrent.SetActive(false);
        pnCurrent = pnNew;
    }

    //-----------------------------------
    // InitGame
    //-----------------------------------
    private void InitGame() {
        // deactive all panels
        foreach (GameObject obj in mPanels) {
            obj.SetActive(false);
        }

        // set active 'PanelButtons'    
        pnCurrent = mPanels.Find(x => x.name.Equals("PanelButtons")).gameObject;
        pnCurrent.SetActive(true);
    }

    //-----------------------------------
    // Button Submit
    //-----------------------------------
    public void OnButtonSubmit(Button button) {
        switch(button.name) {
        case "BtnPlay":
            SceneManager.LoadSceneAsync("MainStage");
            break;

        // every panel's [Back] button will close active panel 
        case "BtnBack":
            ClosePanel();
            break;
        case "BtnQuit":
            Application.Quit();
            break;

        // panel controls
        case "BtnOptions":
            OpenPanel("PanelOptions");
            break;
        case "BtnHiscore":
            OpenPanel("PanelHiscore");
            break;
        case "BtnCredits":
            OpenPanel("PanelCredits");
            break;
        }
    }
}
