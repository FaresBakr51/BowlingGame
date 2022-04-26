using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
public class SetParent :MonoBehaviour
{
    private GameObject _totalScoreLayout;
    [SerializeField] private List<GameObject> _totalScoretexts = new List<GameObject>();

    void Start()
    {
    
        if (this.gameObject.name == "_mytotalscoreprefab(Clone)")
        {
            _totalScoreLayout = GameObject.FindWithTag("totalscorecanavas");
            transform.SetParent(_totalScoreLayout.transform);
            gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else
        {
            StartCoroutine(_GETAlltotalscore());
        }
    }

    IEnumerator _GETAlltotalscore()
    {


        yield return new WaitForSeconds(1.5f);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("totalplayerscore"))
        {
            _totalScoretexts.Add(obj);

        }
        SetCorrectParent();


    }
    private void SetCorrectParent()
    {
        var photonvi = this.GetComponent<PhotonView>();
        transform.SetParent(_totalScoretexts[photonvi.OwnerActorNr-1].transform);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(200, 0, 0);
    }
}
