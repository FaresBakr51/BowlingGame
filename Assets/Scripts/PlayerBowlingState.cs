using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerBowlingState : MonoBehaviourPunCallbacks, PlayerState
{
    private PlayerController playercontroller;
    private bool _goforword;
    public void Handle(PlayerController _playercontroller)
    {
        if (!playercontroller)
        {
            playercontroller = _playercontroller;
        }
        playercontroller._canhit = false;
        ShotBall();
    }
    public void ShotBall()
    {
       
        playercontroller.UpdateAnimator("shot", 1);
        StartCoroutine(WaitGrounded());
        
    }
    private void Update()
    {
        if(_goforword == true)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            this.transform.Translate(Vector3.forward * Time.deltaTime * playercontroller._speed);
            
        }
    }
    IEnumerator WaitGrounded()
    {
        _goforword = true;
        yield return new WaitForSeconds(1.06f);
        playercontroller._ball.GetComponent<Rigidbody>().isKinematic = false;
        playercontroller._ball.transform.parent = null;
     //   playercontroller._ball.transform.Reset();
        _goforword = false;
        playercontroller.UpdateAnimator("shot", 0);
        playercontroller._ball.GetComponent<BallSound>().enabled = true;
        playercontroller._ball.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -playercontroller._power));
        playercontroller._ball.GetComponent<Rigidbody>().AddForce(new Vector3(-playercontroller._driftvalue, 0, 0));

        GameEventBus.Publish(GameEventType.waiting);
    }


}
