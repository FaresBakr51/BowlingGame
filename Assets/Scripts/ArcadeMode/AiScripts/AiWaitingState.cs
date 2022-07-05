using System.Collections;
using UnityEngine;

public class AiWaitingState : AiStates
{
    private AiController _aiController;
    public override void ProcessState(AiController controller)
    {
        _aiController = controller;

        WaitState();
    }

    private void WaitState()
    {
       // _aiController._MyPlayCanavas.SetActive(false);
        if (_aiController._canhit)
        {
            _aiController._canhit = false;
        }
        if (_aiController._ball.activeInHierarchy)
        {
            _aiController._followBall = true;

            _aiController._ball.GetComponent<BallSound>().UpdateSound(_aiController._movingclip);
        }
      
        StartCoroutine(WaitHit());
    }
    IEnumerator WaitHit()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(ChechPins());


    }
    private IEnumerator ChechPins()
    {



        yield return new WaitForSeconds(4f);
        for (int i = 0; i < _aiController._mypins.Count; i++)
        {

            if (_aiController._mypins[i].transform.up.y < 0.9f)//|| playercontroller._resetpins[i].x != playercontroller._mypins[i].transform.localPosition.x)// Mathf.Abs(playercontroller._mypins[i].transform.rotation.eulerAngles.z) > 5f
            {
                if (_aiController._mypins[i].gameObject.activeInHierarchy == true)
                {
                    _aiController._roundscore += 1;
                    _aiController._mypins[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if (_aiController._mypins[i].gameObject.activeInHierarchy == true)
                {
                    _aiController._mypins[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    _aiController._leftpins.Add(_aiController._mypins[i]);
                    _aiController._mypins[i].gameObject.SetActive(false);
                }
            }
        }

        StartCoroutine(Waittopublish());

    }
    IEnumerator Waittopublish()
    {
        yield return new WaitForSeconds(1f);
        _aiController.CheckOtherHit();
    }
}
