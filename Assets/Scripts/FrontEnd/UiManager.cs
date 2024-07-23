using BackEnd;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiManager : Singelton<UiManager> 
{

   


    [SerializeField] private GameObject leaderpardPanel;
    [SerializeField] private Transform contentLeaderboard;
    public Transform ContentLeaderboard { get { return contentLeaderboard; } }
    [SerializeField] private GameObject leaderboardPref;
    public GameObject leaderBoardPrefab { get { return leaderboardPref; } }

    [Header("WriteNamePanel")]
    [SerializeField] private GameObject userNameInputPanel;
    [SerializeField] private TMP_InputField userNameinput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passinput;
    [SerializeField] private TMP_InputField passconfirminput;


    [Header("Login")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private TMP_InputField emailloginInput;
    [SerializeField] private TMP_InputField passlogininput;
   
  
  
    public TMP_InputField UserNameInput { get { return userNameinput; } }


    [SerializeField] private CanvasGroup[] canvasGroups;
    // private bool currentSelectionCheck;


    public UIScreen CurrentUISCreen;
    public GameObject PointerHandler;

 
    public void Leaderboard()
    {
        canvasGroups[0].interactable = false;
        canvasGroups[0].gameObject.SetActive(false);
        foreach (Transform t in contentLeaderboard.transform)
        {
            Destroy(t.gameObject);
        }
       StartCoroutine(DataBaseManager.GetLoadLeaderboard());
        leaderpardPanel.SetActive(true);
    }


    public void ExitLeader()
    {
        canvasGroups[0].interactable = true;
        canvasGroups[0].gameObject.SetActive(true);
    }
    private void Update()
    {
        if (BashAuth.Instance.AuthAction)
        {
            InteractionInputs(false);
        }
        else
        {
            InteractionInputs(true);
        }

        if(CurrentUISCreen == null || !CurrentUISCreen.gameObject.activeInHierarchy)
        {
            if (PointerHandler.activeInHierarchy)
            {
                CurrentUISCreen = FindObjectOfType<UIScreen>();
            }

        }else if(CurrentUISCreen != null && PointerHandler.activeInHierarchy)
        {
            if(EventSystem.current.currentSelectedGameObject ==null || !EventSystem.current.isFocused)
            {
                EventSystem.current.SetSelectedGameObject(CurrentUISCreen.navigationButtons[CurrentUISCreen.currentIndex]);
                CurrentUISCreen.SpecialCaseButton();
            }
        }

//        Debug.Log(EventSystem.current.isFocused);
    }



    public void ExitGame()
    {
        Application.Quit();
    }
    #region Auth
    public void SignUp()
    {
        BashAuth.Instance.SignUp(userNameinput.text,emailInput.text.Trim(), passinput.text.Trim(), passconfirminput.text.Trim());
    }
    public void Login()
    {

        BashAuth.Instance.Login(emailloginInput.text.Trim(), passlogininput.text.Trim());
    }
    public void ClearSignData()
    {
        userNameinput.text = string.Empty;
        emailInput.text = string.Empty;
        passinput.text = string.Empty;
        passconfirminput.text = string.Empty;
    }
    public void ShowInput(TMP_InputField inputfield)
    {

        inputfield.contentType = TMP_InputField.ContentType.Standard;
    }
    public void HidInput(TMP_InputField inputfield)
    {

        inputfield.contentType = TMP_InputField.ContentType.Password;
    }
    public void InteractionInputs(bool enable)
    {
        userNameinput.interactable  = enable;
        emailInput.interactable = enable;
        passinput.interactable = enable;
        emailloginInput.interactable = enable;
        passlogininput.interactable = enable;
        passconfirminput.interactable = enable;
    }
    #endregion
}
