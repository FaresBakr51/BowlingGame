using System.Collections;
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
        playercontroller._timerAfk = 15;


        RpcShotBall();
    }
    
    
    public void RpcShotBall()
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
            if (playercontroller.waitingPUSH)
            {
                var pow = Input.GetAxis("Mouse Y");
                if (pow > 0)
                {
                    Debug.Log("WaitingPush");
                    playercontroller._power = playercontroller.presistencePower;
                    playercontroller.waitingPUSH = false;
                }

            }
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
        playercontroller._ball.GetComponent<BallSound>().enabled = true;
       
        var rig = playercontroller._ball.GetComponent<Rigidbody>();
        rig.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        rig.AddForce(new Vector3(0, 0, -playercontroller._power), ForceMode.Impulse);
        rig.AddForce(new Vector3(-playercontroller._driftvalue, 0, 0),ForceMode.Force);

        rig.AddTorque(Vector3.forward * playercontroller.spinvalue * playercontroller.spinScaler, ForceMode.Impulse);
        rig.AddTorque(  (playercontroller.leftSpin ?  Vector3.left : Vector3.right) * playercontroller.spinvalue * playercontroller.spinScaler, ForceMode.Force);
        rig.AddForce(new Vector3((playercontroller.leftSpin ? playercontroller.spinvalue : -playercontroller.spinvalue), 0, 0), ForceMode.Force);
        playercontroller._ball.GetComponent<TrailRenderer>().enabled = true;
        GameEventBus.Publish(GameEventType.waiting);
    }


}
