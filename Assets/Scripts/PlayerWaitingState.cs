using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitingState : MonoBehaviour,PlayerState
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
        _followBall = true;

        _playercontroller._ballsound.UpdateSound(_playercontroller._movingclip);
        StartCoroutine(WaitHit());
    }
    private void Update()
    {
        if (_followBall == true)
        {
           
            if (playercontroller._camera.transform.position.z >= playercontroller._listpins.transform.position.z +10)
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
        yield return new WaitForSeconds(7f);
        ChechPins();
        

    }
    private void ChechPins()
    {
       for(int i = 0; i < playercontroller._mypins.Count; i++)
        {
            
            if(Mathf.Abs(playercontroller._mypins[i].transform.rotation.eulerAngles.z) > 5f)
            {
                if (playercontroller._mypins[i].gameObject.activeInHierarchy == true)
                {
                    playercontroller._roundscore += 1;
                    playercontroller._mypins[i].gameObject.SetActive(false);
                }
            }
            else
            {
             //   playercontroller._nothitpins.Add(playercontroller._mypins[i].gameObject);
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
