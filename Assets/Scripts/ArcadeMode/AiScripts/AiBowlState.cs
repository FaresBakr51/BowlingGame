using System.Collections;
using UnityEngine;

public class AiBowlState : AiStates
{
    private AiController _aiController;
    private bool _goforword;
    public override void ProcessState(AiController controller)
    {
        _aiController = controller;
        _aiController._canhit = false;
       // _aiController._timerAfk = 15;
        BowlState();
    }

    private void BowlState()
    {
        RpcShotBall();
    }
    public void RpcShotBall()
    {

        _aiController.UpdateAnimator("shot", 1);
        StartCoroutine(WaitGrounded());



    }
    private void Update()
    {
        if (_goforword == true)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            this.transform.Translate(Vector3.forward * Time.deltaTime * _aiController._speed);

        }
    }
    IEnumerator WaitGrounded()
    {
        _goforword = true;
        yield return new WaitForSeconds(1.06f);
        _aiController._ball.GetComponent<Rigidbody>().isKinematic = false;
        _aiController._ball.transform.parent = null;
        _goforword = false;
        _aiController.UpdateAnimator("shot", 0);
        _aiController._ball.GetComponent<BallSound>().enabled = true;
        var rig = _aiController._ball.GetComponent<Rigidbody>();
        rig.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rig.AddForce(new Vector3(0, 0, -_aiController._power), ForceMode.Impulse);
        rig.AddForce(new Vector3(-_aiController._driftvalue, 0, 0), ForceMode.Force);
        _aiController.Waitstate();
     
    }

}
