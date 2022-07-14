using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiResetState : AiStates
{
    private AiController _aiController;
    private Vector3 _mypos;
    public override void ProcessState(AiController _controller)
    {
        _aiController = _controller;
        ResetState();
    }
    private void Awake()
    {

        _mypos = this.transform.position;
    }
    private void ResetState()
    {
        StartCoroutine(WaitToReset());

    }
    private void ResetCamAndpins()
    {
        _aiController._followBall = false;
        _aiController._powerval = false;
        _aiController._hookcalclated = false;
        _aiController._ball.GetComponent<Rigidbody>().isKinematic = true;
        _aiController._ball.GetComponent<BallSound>().enabled = false;
        _aiController._ball.transform.parent = _aiController._playerhand;
       
        _aiController._ball.transform.localPosition = _aiController._BallConstantPos;
        _aiController._roundscore = 0;
        _aiController._ball.GetComponent<BallSound>()._hit = false;
        this.transform.position = _mypos;
        foreach (Transform pins in _aiController._mypins)
        {
            if (pins.name != "PinSetter")
            {
                pins.gameObject.GetComponent<Rigidbody>().isKinematic = false;
             
            }
        }
        if (_aiController._usingRock)
        {
           
             _aiController.RunRpc();
             transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            _aiController.UpdateAnimator("shot", 0);

            if (_aiController._infiniteRockets)
            {
                _aiController._usedRocket = false;
                _aiController.shotType = 0;
            }
            _aiController.shotType = 0;
            _aiController._usingRock = false;

        }

        if (_aiController.shotType != 1)
        {
            _aiController.CheckShotType();
        }
       
    }
    IEnumerator WaitToReset()
    {
  
        if (_aiController._leftpins.Count > 0)
        {
            foreach (Transform leftpin in _aiController._leftpins)
            {
                leftpin.transform.rotation = Quaternion.identity;
                if (leftpin.gameObject.GetComponent<Rigidbody>().isKinematic)
                {
                    leftpin.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                leftpin.gameObject.SetActive(true);
                leftpin.transform.up = new Vector3(leftpin.transform.up.x, 1, leftpin.transform.up.z);
            }
        }
        yield return new WaitForSeconds(3f);
    
        ResetCamAndpins();
        yield return new WaitForSeconds(1f);
         if (_aiController._gameend)
        {

            _aiController._canhit = false;


        }
        else
        {
            _aiController._canhit = true;

        }

      
    }
}
