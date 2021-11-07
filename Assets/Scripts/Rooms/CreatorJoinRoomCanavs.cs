using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorJoinRoomCanavs : MonoBehaviour
{


   [SerializeField]
    private CreatRooms _CreatRoommenu;
    [SerializeField]

    private RoomListingMenu _roomlistinmenu;

    private RoomsController _roomController;
    public void FirstIniatlize(RoomsController _controll){
      
        _roomController  = _controll;
       _CreatRoommenu.FirstIniatlize(_controll);
       _roomlistinmenu.FirstIniatlize(_controll);
    
    }
}
