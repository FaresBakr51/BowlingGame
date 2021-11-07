using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanavas : MonoBehaviour
{
    private RoomsController _roomController;
    public void FirstIniatlize(RoomsController _controll){
        _roomController  = _controll;
    }
    public void Show(){

        gameObject.SetActive(true);
    }
    public void Hide(){

    }
}
