using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Special
{
    public class RocketAbility : SpecialBase
    {
        public override void SpawnAbility(PhotonView sender, Transform point)
        {
            PlayerController controller = sender.GetComponent<PlayerController>();
            controller._usingRock = true;
            controller.transform.position = controller._mypos;
            controller.UpdateAnimator("shot", 2);
            controller.HitRocket();
        }
    }
}