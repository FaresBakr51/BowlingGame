using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class PointerManager : MonoBehaviour
{

    public UIAction currentActionAsset;
   

   
    private void Start()
    {
        currentActionAsset = new UIAction();
        currentActionAsset.UI.Navigate.performed += x=>{

            Debug.Log("Performed");
            if (UiManager.Instance.CurrentUISCreen.currentIndex >= UiManager.Instance.CurrentUISCreen.navigationButtons.Length - 1 )
            {
                UiManager.Instance.CurrentUISCreen.currentIndex = 0;
            }
            else
            {
                UiManager.Instance.CurrentUISCreen.currentIndex++;
            }
           
            EventSystem.current.SetSelectedGameObject(UiManager.Instance.CurrentUISCreen.navigationButtons[UiManager.Instance.CurrentUISCreen.currentIndex]);
            UiManager.Instance.CurrentUISCreen.SpecialCaseButton();


        };
        currentActionAsset.Enable();

    }
    private void OnDisable()
    {
        currentActionAsset.Disable();
    }

}
