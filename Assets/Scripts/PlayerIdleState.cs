using System.Collections;
using UnityEngine;
using Photon.Pun;
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

      
      if(playerController._gameend)
        {
            if(playerController._scoreplayer.totalscre >= 300)
            {
                playerController.UpdateSound(playerController._gameClips[7]);
            }
            playerController._canhit = false;
            if (playerController._MyPlayCanavas.activeInHierarchy)
            {
               playerController._MyPlayCanavas.SetActive(false);
            }
            if (!GameManager.instance._rankedMode)
            {
                playerController._GoHomebutt.SetActive(true);
                playerController.myleader.SetActive(true);
                EventSystem.current.SetSelectedGameObject(playerController._GoHomebutt);
            }
            else
            {
                playerController.CheckWinner();
            }
           
        }
        else
        {
           
            playerController._canhit = true;
        }
       
        if (playerController._myRocket.activeInHierarchy)
        {
        
            playerController._RocketOn.SetActive(false);
            playerController._RocketOff.SetActive(true);
            playerController._usingRock = false;
            playerController.RunRpc();           
           
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            playerController.UpdateAnimator("shot", 0);
            playerController._camera.transform.position = _Cambos;
        }
        playerController._roundscore = 0;
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
