using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 using UnityEngine.Events;
public class CheckButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject){
            this.gameObject.transform.GetChild(1).GetComponent<Image>().enabled = true;

         
        }else{
               this.gameObject.transform.GetChild(1).GetComponent<Image>().enabled = false; 
        }
        
    }
     public void ChangeFunction(UnityAction action ){
    this.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
     this.gameObject.GetComponent<Button>().onClick.AddListener(action);
  }
}
