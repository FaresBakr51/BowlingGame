using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
 
    public Vector3 offset;
    private void Awake()
    {
     
        #region IGT
        transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);
        #endregion
    }

}
