using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
 
    public Vector3 offset;
    public bool IGT;
    private void Awake()
    {

        #region IGT
        if (!IGT) return;
        transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);
        #endregion
    }

}
