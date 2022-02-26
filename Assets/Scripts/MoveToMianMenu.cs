
using UnityEngine;
using UnityEngine.SceneManagement;
public class MoveToMianMenu : MonoBehaviour
{
   public void SkipIntro(){

       SceneManager.LoadScene(0);
   }
}
