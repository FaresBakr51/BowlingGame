using UnityEngine;

public class CurrentRoomCanavas : MonoBehaviour
{
    private RoomsController _roomController;
    [SerializeField]
    private PlayerListingMenu _playerListingMenu;
    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;
    public void FirstIniatlize(RoomsController _controll){
        _roomController  = _controll;
        _playerListingMenu.FirstIniatlize(_controll);
        _leaveRoomMenu.FirstIniatliz(_controll);
    }
    public void Show(){

        gameObject.SetActive(true);
    }
    public void Hide(){
         gameObject.SetActive(false);
    }
}
