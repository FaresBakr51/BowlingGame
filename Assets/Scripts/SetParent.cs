using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
