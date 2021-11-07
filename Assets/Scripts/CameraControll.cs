using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    private Transform target;
    public Vector3 offset;

    public void SetTarget(Transform target)
    {
        this.target = target;
        gameObject.transform.position = target.position + offset;
    }

    public void LateUpdate()
    {
       // gameObject.transform.position = target.position + offset;
    }
}
