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
        yield return new WaitForSeconds(10f);
        ChechPins();
        

    }
    private void ChechPins()
    {
       for(int i = 0; i < playercontroller._mypins.Count; i++)
        {
            
            if(Mathf.Abs(playercontroller._mypins[i].transform.up.y) < 0.9f)
            {
                if (playercontroller._mypins[i].gameObject.activeInHierarchy == true)
                {
                    playercontroller._roundScore += 1;
                    playercontroller._mypins[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playercontroller._nothitpins.Add(playercontroller._mypins[i].gameObject);
            }
        }
        CheckOtherHit();
       

    }

    private void CheckOtherHit()
    {
        if (playercontroller._roundScore >= 10 && playercontroller._turnRounds == 2)
        {
            Debug.Log("strike");
            Strike();
        }
        if( playercontroller._turnRounds == 1 && playercontroller._nothitpins.Count ==10)
        {
            Debug.Log("spare");
            Spare();
        }
      
        AddToLeaderBoard();
        GameEventBus.Publish(GameEventType.reset);
    }
    private void Strike()
    {
        playercontroller._frameRounds += 2;
        playercontroller._turnRounds += 2;
        ResetPins();
    }
    private void Spare()
    {
        playercontroller._nothitpins.Clear();
        playercontroller._frameRounds += 1;
        ResetPins();
    }
    private void AddToLeaderBoard()
    {
        playercontroller._frameRounds -= 1;
        playercontroller._framescore += playercontroller._roundScore;
      
        if (playercontroller._roundScore >= 10)
        {
            for (int i = 0; i < playercontroller._framesText.Count;)
            {
                if (playercontroller._turnRounds == 2)
                {
                    if (playercontroller._framesText[i].text == "")
                    {

                        playercontroller._framesText[i].text = "X";
                        playercontroller._framesText[i+1].text = "-";
                        break;
                    }

                    else
                    {
                        i++;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < playercontroller._framesText.Count;)
            {
                if (playercontroller._framesText[i].text == "")
                {
                    playercontroller._framesText[i].text = playercontroller._roundScore.ToString();
                    break;
                }
                else
                {
                    i++;
                   
                }

            }
        }
        if(playercontroller._frameRounds <= 0)
        {

            for (int i = 0; i < playercontroller._framescoretext.Count;)
            {
                if (playercontroller._framescoretext[i].text == "")
                {
                    playercontroller._framescoretext[i].text = playercontroller._framescore.ToString();
                    break;
                }
                else
                {
                    i++;

                }

            }
           
            playercontroller._totalScore += playercontroller._framescore;
            playercontroller._totaleScoreTxt.text = playercontroller._totalScore.ToString();
            playercontroller._frameRounds = 2;
            playercontroller._framescore = 0;
        }
        playercontroller._turnRounds -= 1;
        if (playercontroller._turnRounds <= 0)
        { 
            playercontroller._turnRounds = 2;
            ResetPins();
        }
            
    }
    private void ResetPins()
    {
            for (int y = 0; y < playercontroller._resetpins.Count; y++)
            {
            if (playercontroller._mypins[y].gameObject.activeInHierarchy == false)
            {
                playercontroller._mypins[y].gameObject.SetActive(true);
            }
            playercontroller._mypins[y + 1 - 1].gameObject.GetComponent<Rigidbody>().isKinematic = true;
            playercontroller._mypins[y+1-1].transform.position = playercontroller._resetpins[y+1-1].transform.position;
            playercontroller._mypins[y+1-1].transform.rotation = playercontroller._resetpins[y+1-1].transform.rotation;
            
            }
    }
    
}
