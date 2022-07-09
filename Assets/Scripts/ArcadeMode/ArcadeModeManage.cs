using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArcadeModeManage : MonoBehaviour
{
    public Transform _spawnPoint;
    [SerializeField] private AiController _currentAi;
    [SerializeField] private PlayerController _currentPlayer;
    [SerializeField] private GameObject _vsPanel;
    [SerializeField] private Image _previewbotImage;
    [SerializeField] private Image _previewplayerImage;
    [SerializeField] private Sprite[] _bowlerSprites;
  
    //[SerializeField] private GameObject[]
    [SerializeField] private bool _getnextAi;
    private bool _checkWinner;

    private void OnEnable()
    {
        if (GameModes._arcadeMode)
        {
            GameEventBus.Subscribe(GameEventType.arcademode, GetToNextAi);
        }
    }
    private void OnDisable()
    {
        if (GameModes._arcadeMode)
        {
            GameEventBus.UnSubscribe(GameEventType.arcademode, GetToNextAi);
        }
    }
    void Start()
    {
        if (GameModes._arcadeMode)
        {
          
            StartCoroutine(RunArcadeGame());
        }
    }

 
    private void Update()
    {
        if (_currentAi == null || _currentPlayer == null) return;
        if (_currentAi._gameend && _currentPlayer._gameend && _checkWinner)
        {

           
            CheckWinner();
            _checkWinner = false;
        }
    }
    public void GetToNextAi()
    {
        _checkWinner = true;
        Debug.Log("checking winner");
    }
    private void CheckWinner()
    {
        Debug.Log("Checkarcade winner");
        if(_currentAi._scoreplayer.totalscre > _currentPlayer._scoreplayer.totalscre)
        {
            
            //botwinner dont get next one 
        }
        else if(_currentAi._scoreplayer.totalscre < _currentPlayer._scoreplayer.totalscre)
        {
            _getnextAi = true;
            //playerwinnget next
        }
        else
        {
            _getnextAi = true;
            //draw go next
        }
       
        if (_getnextAi)
        {
            var lastai = PlayerPrefs.GetInt("ai", 0);
            PlayerPrefs.SetInt("ai", lastai + 1);
            PlayerPrefs.Save();
            GetAiNameDependOnIndex(PlayerPrefs.GetInt("ai"));
            StartCoroutine(GetNextAiGame());
        }
    }
    public void GetAiNameDependOnIndex(int indx)
    {

        switch (indx)
        {
            case 1:
                PlayerPrefs.SetString("selectedai", "AiBarney");
                break;
            case 2:
                PlayerPrefs.SetString("selectedai", "AiCarl");
                break;
            case 3:
                PlayerPrefs.SetString("selectedai", "AiCindy");
                break;
            case 4:
                PlayerPrefs.SetString("selectedai", "AiIzzy");
                break;
            case 5:
                PlayerPrefs.SetString("selectedai", "AiJong");
                break;
            case 6:
                PlayerPrefs.SetString("selectedai", "AiTwig");
                break;
            case 7:
                PlayerPrefs.SetString("selectedai", "AiBill");
                break;

        }
        PlayerPrefs.Save();
    }
    IEnumerator GetNextAiGame()
    {
        yield return new WaitForSeconds(3f);
        if (PlayerPrefs.GetInt("ai") < 7)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
   IEnumerator RunArcadeGame()
    {
        _vsPanel.SetActive(true);
        _previewbotImage.sprite = _bowlerSprites[PlayerPrefs.GetInt("ai", 0)];
        _previewplayerImage.sprite = _bowlerSprites[PlayerPrefs.GetInt("selectedplayerindx",0)];
     
        yield return new WaitForSeconds(3f);

        _vsPanel.SetActive(false);
        PhotonNetwork.Instantiate("PhotonNetworkAvatar", transform.position, transform.rotation, 0);
        PhotonNetwork.Instantiate("PhotonAiAvatar", _spawnPoint.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        _currentAi = GameObject.FindGameObjectWithTag("Ai").GetComponent<AiController>();
        _currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
     
    }

    
}
