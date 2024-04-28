using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
public class PlayerWaitingState : MonoBehaviourPunCallbacks,PlayerState
{
    private PlayerController playercontroller;
    private void Awake()
    {
        playercontroller = GetComponent<PlayerController>();
    }
        public void Handle(PlayerController _playercontroller)
    {
        if (!playercontroller)
        {
            playercontroller = _playercontroller;
        }
        playercontroller._MyPlayCanavas.SetActive(false);
        if (playercontroller._canhit)
        {
            playercontroller._canhit = false;
        }
        if (playercontroller._ball.activeInHierarchy) { 
        playercontroller._followBall = true;

        _playercontroller._ball.GetComponent<BallSound>().UpdateSound(_playercontroller._movingclip);
        }
        if (!playercontroller._usingRock)
        {

            playercontroller.UpdateAnimator("shot", 7);
        }
      
        playercontroller._driftBall = true;
        StartCoroutine(WaitHit());
    }
    private void Update()
    {
        if (playercontroller._followBall)
        {
           
            if (playercontroller._camera.transform.position.z >= playercontroller._mypinsobj.transform.position.z + 10)
            {
                playercontroller._camera.transform.position = playercontroller._ball.transform.position + new Vector3(0f, 1f, 1f);
             
                //playercontroller._camera.transform.localEulerAngles = new Vector3(30, -135, playercontroller._camera.transform.localEulerAngles.z);
            }
            else
            {
                playercontroller._followBall = false;
            }
           
        }
    }
    IEnumerator WaitHit()
    {
      yield return new WaitForSeconds(5f);
       StartCoroutine(ChechPins());
        

    }
    private IEnumerator  ChechPins()
    {


       
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < playercontroller._mypins.Count; i++)
        {
            Debug.LogError(playercontroller._mypins[i].transform.eulerAngles.y);
            if (playercontroller._mypins[i].transform.eulerAngles.y > 10 || playercontroller._mypins[i].transform.eulerAngles.y > 10 || playercontroller._mypins[i].transform.eulerAngles.z > 10) //up.y < 0.9f)//|| playercontroller._resetpins[i].x != playercontroller._mypins[i].transform.localPosition.x)// Mathf.Abs(playercontroller._mypins[i].transform.rotation.eulerAngles.z) > 5f
            {
                if (playercontroller._mypins[i].gameObject.activeInHierarchy == true)
                {
                    playercontroller._roundscore += 1;
                    playercontroller._mypins[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if (playercontroller._mypins[i].gameObject.activeInHierarchy == true)
                {
                    playercontroller._mypins[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    playercontroller._leftpins.Add(playercontroller._mypins[i]);
                    playercontroller._mypins[i].gameObject.SetActive(false);
                }
            }
        }

        StartCoroutine(Waittopublish());

    }
    IEnumerator Waittopublish()
    {
        yield return new WaitForSeconds(1f);
        GameEventBus.Publish(GameEventType.leaderboard);
    }

   
    
}
