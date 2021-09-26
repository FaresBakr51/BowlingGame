using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : MonoBehaviour,PlayerState
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
        playerController._hookcalclated = false;
        playerController._ball.GetComponent<Rigidbody>().isKinematic = true;
        playerController._ball.transform.parent = playerController._playerhand;
        playerController._slidertime = 0;
        playerController._scrolltime = 0;
        playerController._powerSlider.value = 0;
        playerController._hookScroll.value = 0.5f;
        playerController._camera.transform.position = _Cambos;
        playerController._ball.transform.localPosition = playerController._BallConstantPos;
        playerController._canhit = true;
        foreach(Transform pins in playerController._mypins)
        {
            pins.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(1f);
        if (playerController._powersliderobj.activeInHierarchy == false) { 
        
            playerController._powersliderobj.SetActive(true);
        }
        if (playerController._hookScrollobj.activeInHierarchy == false)
        {
            playerController._hookScrollobj.SetActive(true);
        }
        playerController._MyPlayCanavas.SetActive(false);
        playerController._MyLeaderBoardCanavas.SetActive(true);
        yield return new WaitForSeconds(3f);
        playerController._MyLeaderBoardCanavas.SetActive(false);
        playerController._MyPlayCanavas.SetActive(true);
        ResetCamAndpins();
       
    }
}
