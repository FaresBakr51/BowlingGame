using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayerIdleState : MonoBehaviourPunCallbacks,PlayerState
{
    private PlayerController playerController;
    private Vector3 _Cambos;
    private Vector3 _mypos;
  
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        _mypos = this.transform.position;
        _Cambos = playerController._camera.transform.position;
    }
    public void Handle(PlayerController _playercontroller)
    {
        if (!playerController)
        {
            playerController = _playercontroller;
        }
        this.transform.position = _mypos;
        IdleState();
    }

    private void IdleState()
    {
        StartCoroutine(WaitToReset());
    }
    private void ResetCamAndpins()
    {
        playerController._powerval = false;
        playerController._hookcalclated = false;
        playerController._powerSlider.gameObject.SetActive(false);
        playerController._hookScroll.gameObject.SetActive(true);
        playerController._calcPower = false;
        playerController._ball.GetComponent<Rigidbody>().isKinematic = true;
        playerController._ball.GetComponent<BallSound>().enabled = false;
        playerController._ball.transform.parent = playerController._playerhand;
        playerController._slidertime = 0;
        playerController._scrolltime = 0;
        playerController._powerSlider.value = 0;
        playerController._hookScroll.value = 0.5f;
        playerController._camera.transform.position = _Cambos;
        playerController._ball.transform.localPosition = playerController._BallConstantPos;
      if(playerController._gameend == true)
        {
            playerController._canhit = false;
            playerController.myleader.SetActive(true);
            playerController._GoHomebutt.SetActive(true);
               EventSystem.current.SetSelectedGameObject(playerController._GoHomebutt);
           // GameManager.instance.RequestFinalLeaderBoarD();
        }
        else
        {
            playerController._canhit = true;
        }
       
        playerController._roundscore = 0;
      //  playerController.increase_round();
        playerController._ball.GetComponent<BallSound>()._hit = false;
        foreach (Transform pins in playerController._mypins)
        {
            pins.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
     
    }

    IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(1.5f);
        playerController.myleader.SetActive(true);
        // GameManager.instance.RequestLeaderBoard();
        yield return new WaitForSeconds(3f);
        playerController.myleader.SetActive(false);
        playerController._MyPlayCanavas.SetActive(true);
        ResetCamAndpins();
       
    }
}
