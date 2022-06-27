using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiResetState : AiStates
{
    private AiController _aiController;
    private Vector3 _Cambos;
    private Vector3 _mypos;
    public override void ProcessState(AiController _controller)
    {
        _aiController = _controller;
        ResetState();
    }
    private void Awake()
    {

        _mypos = this.transform.position;
        // _Cambos = _aiController._camera.transform.position;
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
        _aiController._powerSlider.gameObject.SetActive(false);
        _aiController._hookScroll.gameObject.SetActive(true);
        _aiController._calcPower = false;
        _aiController._ball.GetComponent<Rigidbody>().isKinematic = true;
        _aiController._ball.GetComponent<BallSound>().enabled = false;
        _aiController._ball.transform.parent = _aiController._playerhand;
        _aiController._slidertime = 0;
        _aiController._scrolltime = 0;
        _aiController._powerSlider.value = 0;
        _aiController._hookScroll.value = 0.5f;
       
        _aiController._ball.transform.localPosition = _aiController._BallConstantPos;
        _aiController._roundscore = 0;
        _aiController._ball.GetComponent<BallSound>()._hit = false;
        this.transform.position = _mypos;


        if (_aiController._gameend)
        {

            _aiController._canhit = false;
            if (_aiController._MyPlayCanavas.activeInHierarchy)
            {
                _aiController._MyPlayCanavas.SetActive(false);
            }

      
        }
        else
        {
            //   _aiController._timerAfk = 15;

            _aiController._canhit = true;
        
        }





        foreach (Transform pins in _aiController._mypins)
        {
            if (pins.name != "PinSetter")
            {
                pins.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                //   pins.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }

    }
    IEnumerator WaitToReset()
    {
      //  _aiController.myleader.SetActive(true);
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
      //  if (_aiController.myleader.activeInHierarchy) { _aiController.myleader.SetActive(false); }
        _aiController._MyPlayCanavas.SetActive(true);
        ResetCamAndpins();
    }
}
