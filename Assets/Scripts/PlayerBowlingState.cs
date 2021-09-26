using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowlingState : MonoBehaviour, PlayerState
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
            this.transform.Translate(Vector3.forward * Time.deltaTime * playercontroller._speed);
        }
    }
    IEnumerator WaitGrounded()
    {
        _goforword = true;
        yield return new WaitForSeconds(1.06f);
        playercontroller._ball.GetComponent<Rigidbody>().isKinematic = false;
        playercontroller._ball.transform.parent = null;
        _goforword = false;
        playercontroller.UpdateAnimator("shot", 0);
        playercontroller._ball.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -playercontroller._power));
        playercontroller._ball.GetComponent<Rigidbody>().AddForce(new Vector3(-playercontroller._driftvalue, 0, 0));
        GameEventBus.Publish(GameEventType.waiting);
    }


}
