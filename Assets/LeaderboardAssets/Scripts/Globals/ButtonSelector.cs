//-----------------------------------
//    Select first button of each panels
//
//      Last Modified : 2021.03.08 
//       seungje.park@iircade.com
//-----------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour {

    [SerializeField] Button btnSelect;

    //-----------------------------------
    // OnEnabled
    //-----------------------------------
    void OnEnable() {
        StartCoroutine(ButtonSelect());
    }

    //-----------------------------------
    // Button Select
    //-----------------------------------
    IEnumerator ButtonSelect() {
        // wait a little bit time. maybe Unity EventSystem bug.... 
        yield return new WaitForSeconds(0.02f);

        if (btnSelect != null) {
            EventSystem.current.SetSelectedGameObject(null);  
            btnSelect.Select();
        }
    }
}
