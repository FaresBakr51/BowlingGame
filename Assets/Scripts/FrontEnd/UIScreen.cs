using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScreen : Singelton<UIScreen>
{
    [Header("Screen Buttons")]
    public int currentIndex;
    public GameObject[] navigationButtons;

    [Header("OtherCases")]
    public SpecialButton[] specialButtons;
    //public GameObject hideButton;
    //public GameObject showButton;
    public void SpecialCaseButton()
    {

        foreach(var special in specialButtons)
        {
            for(int i = 0;i< special.buttons.Length;i++)
            {
                if (EventSystem.current.currentSelectedGameObject == special.buttons[i] && !special.buttons[i].activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(special.buttons[i+1]);
                    break;
                }
            }
       
        }
        //if (EventSystem.current.currentSelectedGameObject == hideButton && !hideButton.activeInHierarchy)//ShowButton
        //{
        //    Debug.Log("HideButton Selected Switch");
        //    EventSystem.current.SetSelectedGameObject(showButton);
        //}else if (EventSystem.current.currentSelectedGameObject == showButton && !showButton.activeInHierarchy)
        //{
        //    Debug.Log("Show Button Selected");
        //    EventSystem.current.SetSelectedGameObject(hideButton);
        //}
    }

}
[System.Serializable]
public struct SpecialButton
{
    public GameObject[] buttons;
}
