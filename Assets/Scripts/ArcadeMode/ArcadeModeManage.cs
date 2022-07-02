using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArcadeModeManage : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    void Start()
    {
        if (GameModes._arcadeMode)
        {
          //  PhotonNetwork.Instantiate("PhotonAiAvatar",_spawnPoint.position,transform.rotation);
        }
    }

  
}
