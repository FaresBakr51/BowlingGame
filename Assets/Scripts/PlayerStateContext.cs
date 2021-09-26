using UnityEngine;

public class PlayerStateContext : MonoBehaviour
{
   public PlayerState CurrentState
    {
        get; set;
    }
    private readonly PlayerController _playercontroller;
    public PlayerStateContext(PlayerController playercontroller)
    {
        _playercontroller = playercontroller;
    }
  public void Transition(PlayerState state)
    {
        CurrentState = state;
        CurrentState.Handle(_playercontroller);
    }
}
