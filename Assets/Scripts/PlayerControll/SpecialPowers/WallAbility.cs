using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;

namespace Special
{
    public class WallAbility : SpecialBase
    {
       
        [SerializeField] private List<GameObject> wallPoints = new List<GameObject>();


        private void Start()
        {
          
        }
        public override void SpawnAbility(PhotonView sender,Transform point)
        {
            wallPoints = GameObject.FindGameObjectsWithTag("spawnPointwall").ToList();
            if (wallPoints.Count > 0)
            {
                Debug.Log("Spawning Wall");
                int rand = Random.Range(0, wallPoints.Count);
                Debug.Log(wallPoints[rand].transform.position);
                Debug.Log(point.position);
                if (point)
                {
                    if (point.transform.position == wallPoints[rand].transform.position)
                    {
                        Debug.Log("Same as player wall point Removing !!");
                        //same as the player wall point

                      //  wallPoints.Remove(wallPoints[rand]);//remove it from the list
                        if (wallPoints.Count > 0)
                        {
                            rand = Random.Range(0, wallPoints.Count);
                            point = wallPoints[rand].transform;
                        }
                        else//no more points
                        {
                            point = null;
                        }

                    }
                    else
                    {
                        point = wallPoints[rand].transform;
                    }
                }
                if (point == null) return;
                SpawnWall(point.position, point.rotation);
            }


        }

      
        private void SpawnWall(Vector3 pos,Quaternion rot)
        {
            PhotonNetwork.Instantiate("wallprefab", pos, rot);

        }
    }
}
