using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {

    private bool simUlateSpin;
    private float spinValue;
    private Rigidbody rig;
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    void FixedUpdate() {

        if(spinValue == 0) return;
      
        SimulateSpin(spinValue);
    }

    private void SimulateSpin(float spinValue)
    {
        if (!simUlateSpin) return;
        Debug.Log("Simulating spin");
        Debug.DrawRay(transform.position, Vector3.right * 5, Color.red, 2f);

        rig.AddTorque(Vector3.forward* spinValue, ForceMode.Force);
        rig.AddTorque(Vector3.up * spinValue, ForceMode.Force);
    }

    public void OnStartSpin(bool spin,float spinval)
    {
        if (spin)
        {
            
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
         
        }
        simUlateSpin = spin;
        spinValue = spinval;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("pin"))
        {
            simUlateSpin = false;
        }


    }
}
