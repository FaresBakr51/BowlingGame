using UnityEngine;
using UnityEngine.UI;
public class RoyalModeState : MonoBehaviour, GameModeState
{
    private PlayerController _controller;
    public void GameMode(PlayerController controll)
    {
        _controller = controll;
    }
    void Update()
    {
        if (_controller.photonView.IsMine)
        {
            if (_controller._canhit && _controller._battleStart)
            {
                HandleTimeOut();
            }
            HandleBattleGame();
        }
    }
    private void HandleBattleGame()
    {
        if (!_controller._battleStart)
        {

            _controller.battletimer -= Time.deltaTime;

            if (_controller.battletimer <= 3 && _controller.battletimer > 0)
            {
                _controller._battleRoyalDescrypt.GetComponent<Text>().text = ((int)_controller.battletimer).ToString();
            }
            else if (_controller.battletimer <= 0 && _controller.battletimer > -1)
            {
                _controller._battleRoyalDescrypt.GetComponent<Text>().text = "GO !";
            }
            else if (_controller.battletimer <= -1)
            {
                _controller._battleRoyalDescrypt.SetActive(false);
                _controller._battleStart = true;
            }

        }
        if (_controller._modePlayers.Count > 0 && _controller._checkIfthereOther)
        {

            if (_controller.IamTheOnlyOne())
            {
                if (!_controller._gameend)
                {
                    _controller._gameend = true;
                    _controller.ShowRankedResult("win2");
                    _controller._checkIfthereOther = false;
                }
            }
        }

    }
    private void HandleTimeOut()
    {

        _controller._timerAfk -= Time.deltaTime;
        if (_controller._timerAfk <= 0 && _controller._checkIfthereOther)
        {

            if (!_controller._gameend)
            {
                _controller._gameend = true;
                _controller.ShowRankedResult("timeout");
                _controller._checkIfthereOther = false;
            }
        }
    }
}
