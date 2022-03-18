using UnityEngine;
using UnityEngine.UI;
public class SetParent :MonoBehaviour
{
    private GameObject _totalScoreLayout;

    void Start()
    {
          _totalScoreLayout = GameObject.FindWithTag("totalscorecanavas");
          transform.SetParent(_totalScoreLayout.transform);
           gameObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
     
    }
   


}
