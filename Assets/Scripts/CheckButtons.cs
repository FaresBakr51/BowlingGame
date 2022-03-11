using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 using UnityEngine.Events;
public class CheckButtons : MonoBehaviour
{

    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject){
            this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;

         
        }else{
               this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false; 
        }
        
    }
     public void ChangeFunction(UnityAction action ){
    this.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
     this.gameObject.GetComponent<Button>().onClick.AddListener(action);
  }
}
