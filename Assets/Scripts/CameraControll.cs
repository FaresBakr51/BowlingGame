using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
 
    public Vector3 offset;
    public bool SetIGTCAM;

    private void Start()
    {
        if (SetIGTCAM)
        {
            transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);
        }
    }

}
