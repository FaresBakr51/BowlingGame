using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
public class PlayerWaitingState : MonoBehaviourPunCallbacks,PlayerState
{
    private PlayerController playercontroller;
    private bool _followBall;
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
        _followBall = true;

        _playercontroller._ball.GetComponent<BallSound>().UpdateSound(_playercontroller._movingclip);
        }
        StartCoroutine(WaitHit());
    }
    private void Update()
    {
        if (_followBall == true)
        {
           
            if (playercontroller._camera.transform.position.z >= playercontroller._mypinsobj.transform.position.z +10)
            {
                playercontroller._camera.transform.position = playercontroller._ball.transform.position + new Vector3(0, 1, 1);
            }
            else
            {
                _followBall = false;
            }
           
        }
    }
    IEnumerator WaitHit()
    {
        yield return new WaitForSeconds(8f);
        ChechPins();
        

    }
    private void ChechPins()
    {

      

        for (int i = 0; i < playercontroller._mypins.Count; i++)
        {

            if (playercontroller._mypins[i].transform.up.y < 0.85f )//|| Mathf.Abs(playercontroller._mypins[i].transform.rotation.eulerAngles.z) > 5f|| Mathf.Abs(playercontroller._mypins[i].transform.rotation.eulerAngles.x) > 5f)//Mathf.Abs(playercontroller._mypins[i].transform.rotation.eulerAngles.z) > 5f)
            {
                if (playercontroller._mypins[i].gameObject.activeInHierarchy == true)
                {
                    playercontroller._roundscore += 1;
                    playercontroller._mypins[i].gameObject.SetActive(false);
                }
            }
            else
            {
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
