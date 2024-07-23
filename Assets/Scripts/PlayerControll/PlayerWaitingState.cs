using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
public class PlayerWaitingState : MonoBehaviourPunCallbacks,PlayerState
{
    private PlayerController playercontroller;
    [SerializeField] private Vector3 camOffset = new Vector3(0,0.4f,0.3f);
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
                playercontroller._camera.transform.position = playercontroller._ball.transform.position + camOffset/*new Vector3(0f, 1f, 1f)*/;
             
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


       
        yield return new WaitForSeconds(2f);
        List<Transform> droppedPins = new List<Transform>();
        for (int i = 0; i < playercontroller._mypins.Count; i++)
        {
            Debug.LogError(playercontroller._mypins[i].transform.eulerAngles.y);
            if (playercontroller._mypins[i].transform.eulerAngles.y > 10 || playercontroller._mypins[i].transform.eulerAngles.y > 10 || playercontroller._mypins[i].transform.eulerAngles.z > 10) //up.y < 0.9f)//|| playercontroller._resetpins[i].x != playercontroller._mypins[i].transform.localPosition.x)// Mathf.Abs(playercontroller._mypins[i].transform.rotation.eulerAngles.z) > 5f
            {
                if (playercontroller._mypins[i].gameObject.activeInHierarchy == true)
                {
                    playercontroller._roundscore += 1;
                    droppedPins.Add(playercontroller._mypins[i]);
                   // playercontroller._mypins[i].gameObject.SetActive(false);//remove the dropped Pins thats calculated
                }
            }
            else
            {
                
                if (playercontroller._mypins[i].gameObject.activeInHierarchy == true)
                {
                  
                    playercontroller._mypins[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    playercontroller._leftpins.Add(playercontroller._mypins[i]);
                   // playercontroller._mypins[i].gameObject.SetActive(false);
                }
            }
        }



        CleanAnimation(playercontroller._roundscore, playercontroller._leftpins,droppedPins);

    }

    private void CleanAnimation(int roundScore, List<Transform> leftpins,List<Transform> droppedPins)
    {
        if(roundScore < 10 && leftpins.Count > 0)
        {
            playercontroller.myPinsSetter.transform.Find("Collector").gameObject.SetActive(true);
            playercontroller.myPinsSetter.GetComponent<Animator>().Play("Down");
            StartCoroutine(WaitUp(roundScore,1.5f, leftpins,droppedPins));
        }
        else
        {
            Cleaner(roundScore,leftpins,droppedPins);
           // StartCoroutine(WaitUp(0, leftpins, droppedPins));
        }
    }
    IEnumerator WaitUp(int roundScore, float waittime,List<Transform> leftpins, List<Transform> droppedPins)
    {
        yield return new WaitForSeconds(0.6f);//attach pins to setter //DOWN Time to attach
        foreach (Transform pin in leftpins)
            pin.transform.parent = playercontroller.myPinsSetter.transform.Find("Collector");//Attach Pins to collector
        yield return new WaitForSeconds(waittime);
        Cleaner(roundScore, leftpins,droppedPins);
    }
    private void Cleaner(int roundScore, List<Transform> leftpins, List<Transform> droppedPins)
    {
        playercontroller.myPinsSetter.transform.Find("Pins_Cleaner").gameObject.SetActive(true);//Active Cleaner
        playercontroller.myPinsSetter.GetComponent<Animator>().Play("Clean");
        StartCoroutine(Waittopublish(roundScore,droppedPins, leftpins));
    }
    IEnumerator Waittopublish(int roundScore, List<Transform> droppedPins, List<Transform> leftpins)
    {
        yield return new WaitForSeconds(1.5f);//Clean Time

        foreach (Transform pin in droppedPins)//clean dropped pins UnActive
            pin.gameObject.SetActive(false);
        playercontroller.myPinsSetter.transform.Find("Pins_Cleaner").gameObject.SetActive(false);
        if (roundScore <10)
        {
           
          //  playercontroller.myPinsSetter.transform.Find("Pins_Cleaner").gameObject.SetActive(false);//Remove Cleaner
            playercontroller.myPinsSetter.GetComponent<Animator>().Play("Down");
            yield return new WaitForSeconds(0.6f); // down time

            foreach (Transform pin in leftpins)//Reback Pins Parent
                pin.transform.parent = playercontroller._mypinsobj.transform;
           
            yield return new WaitForSeconds(0.6f); //up time
            playercontroller.myPinsSetter.transform.Find("Collector").gameObject.SetActive(false);
        }
   
      
        yield return new WaitForSeconds(1.5f);//1
        GameEventBus.Publish(GameEventType.leaderboard);
    }

   
    
}
