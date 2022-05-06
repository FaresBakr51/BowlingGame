using UnityEngine;

public class RankedModeState : MonoBehaviour, GameModeState
{
    private PlayerController _controller;
    public void GameMode(PlayerController controll)
    {
        _controller = controll;
        throw new System.NotImplementedException();
    }
    void Update()
    {
        if (!_controller.photonView.IsMine) return;
        if (_controller._gameend && !_controller._gameRankedFinished && !_controller._rankedPanel.activeInHierarchy)
        {
            _controller.CheckWinner();

        }

        else if (_controller._modePlayers.Count > 0 && _controller._checkIfthereOther)
        {
            if (_controller._modePlayers[0] == null)
            {
                if (!_controller._gameend && !_controller._rankedPanel.activeInHierarchy)
                {
                    _controller._gameend = true;
                    _controller.ShowRankedResult("win");
                    _controller._checkIfthereOther = false;
                }
            }
        }
    }
}
