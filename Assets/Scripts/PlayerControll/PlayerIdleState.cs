using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
public class PlayerIdleState : MonoBehaviourPunCallbacks,PlayerState
{
    private PlayerController playerController;
    private Vector3 _Cambos;
 
    private Vector3 _mypos;
  
    private void Start()
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
     
        IdleState();
     
    }

    private void IdleState()
    {
        StartCoroutine(WaitToReset());
    }
    private void ResetCamAndpins()
    {
        Debug.LogWarning("Reset Pins For Next Shot");
        playerController._followBall = false;
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
        playerController._driftBall = false;
        playerController._ball.transform.localPosition = playerController._BallConstantPos;
        playerController._roundscore = 0;
        playerController._ball.GetComponent<BallSound>()._hit = false;
        this.transform.position = _mypos;
        playerController._camera.transform.position = _Cambos;
        if (playerController.IGT || playerController._trackBall)
        {

            if (playerController.IGT)
            {
                playerController._filledImage.fillAmount = 0;
            }
            else if (playerController._trackBall)
            {
                playerController.waitingPUSH = false;
                playerController._waitTrackBall = false;
                playerController._trackBallImage.fillAmount = 0;
            }
            playerController.force = 0;
        }

        playerController._ball.GetComponent<TrailRenderer>().enabled = false;
     
        if (playerController._gameend)
        {
            playerController.UpdateAnimator("shot", 0);
            if (playerController._scoreplayer.totalscre >= 300)
            {
                playerController.UpdateSound(playerController._gameClips[7]);
            }
            playerController._canhit = false;
            if (playerController._MyPlayCanavas.activeInHierarchy)
            {
               playerController._MyPlayCanavas.SetActive(false);
            }

            if (!GameModes._rankedMode && !GameModes._arcadeMode)
            {
                playerController._GoHomebutt.SetActive(true);
                playerController.myleader.SetActive(true);
                EventSystem.current.SetSelectedGameObject(playerController._GoHomebutt);
            }

            else if (GameModes._rankedMode)
            {
                if (!playerController._rankedPanel.activeInHierarchy)
                {
                    playerController.CheckWinner();
                }
            }
            //}else if (GameModes._arcadeMode)
            //{
            //    playerController.UpdateAnimator("shot", 0);
            //}
           
        }
        else
        {

            playerController._MyPlayCanavas.SetActive(true);
            if (GameModes._battleRoyale)
            {
                playerController._timerAfk = 15;
            }
            if (playerController._dance && !playerController._usingRock)
            {

                playerController.RunRpcDance(false);

                int rand = Random.Range(3, 7);
                AnimationClip varclip;
                if (playerController._DanceClips.TryGetValue(rand, out varclip))
                {
                    playerController.UpdateAnimator("shot", rand);
                    StartCoroutine(WaitDanceMotion(varclip.length));

                }
            }
            else
            {
                playerController.UpdateAnimator("shot", 0);
                playerController._canhit = true;
             
            }
            
        }
        if (playerController.usedAbility && playerController._RocketOn.activeInHierarchy)
        {
            playerController._RocketOn.SetActive(false);
            playerController._RocketOff.SetActive(true);
        }
    
        if (playerController._usingRock)
        {
           
            playerController._RocketOn.SetActive(false);
            playerController._RocketOff.SetActive(true);
            playerController._usingRock = false;
            playerController.RunRpc();           
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            playerController.UpdateAnimator("shot", 0);
            playerController._camera.transform.position = _Cambos;
          
        }
        foreach (Transform pins in playerController._mypins)
        {
            if (pins.name != "PinSetter")
            {
                pins.gameObject.GetComponent<Rigidbody>().isKinematic = false;  
            }
        }
      


    }
  
    IEnumerator WaitDanceMotion(float length)
    {
       
        yield return new WaitForSeconds(length -0.1f);
       
      
        playerController.UpdateAnimator("shot", 0);
        playerController.RunRpcDance(true);
        if (playerController._dance) { playerController._dance = false; }
        playerController._canhit = true;
    }
    IEnumerator WaitToReset()
    {
        if (!GameModes._battleRoyale) { playerController.myleader.SetActive(true); }
        if (playerController._leftpins.Count > 0)
        {
            foreach (Transform leftpin in playerController._leftpins)
            {
                leftpin.transform.rotation = Quaternion.identity;
                if (leftpin.gameObject.GetComponent<Rigidbody>().isKinematic)
                {
                    leftpin.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                leftpin.gameObject.SetActive(true);
                leftpin.transform.up = new Vector3(leftpin.transform.up.x, 1, leftpin.transform.up.z);
            }
            playerController._leftpins.Clear();
        }
        yield return new WaitForSeconds(3f);
        if (playerController.myleader.activeInHierarchy) { playerController.myleader.SetActive(false); }
        
        ResetCamAndpins();
       
    }
}
