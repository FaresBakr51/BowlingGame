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
   
}
