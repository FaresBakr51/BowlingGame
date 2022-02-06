using UnityEngine;

public class RoomsController : MonoBehaviour
{
    [SerializeField] private CreatorJoinRoomCanavs _creatorJoinRoomCanavas;
    public CreatorJoinRoomCanavs CreatorJoinRoomCanavs{get{ return _creatorJoinRoomCanavas;}}
    [SerializeField] private CurrentRoomCanavas _currentRoomCanavas;
     public CurrentRoomCanavas CurrentRoomCanavas{get{ return _currentRoomCanavas;}}

     void Awake(){

         FirstIniatlize();
     }
     private void FirstIniatlize(){

         CreatorJoinRoomCanavs.FirstIniatlize(this);
         CurrentRoomCanavas.FirstIniatlize(this);
     }
}
