using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class ArcadeModeManage : MonoBehaviour
{
    public Transform _spawnPoint;
    [SerializeField] private AiController _currentAi;
    [SerializeField] private PlayerController _currentPlayer;
    [SerializeField] private GameObject _vsPanel;
    [SerializeField] private Image _previewbotImage;
    [SerializeField] private Image _previewplayerImage;
    [SerializeField] private Sprite[] _bowlerSprites;
    [SerializeField] private Sprite[] _bowlerSpritesAi;
    private bool _checkWinner;
    private bool _clicked;
    private void OnEnable()
    {
        if (GameModes._arcadeMode)
        {
            _clicked = false;
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

    }
    private void CheckWinner()
    {

        if (_currentAi._scoreplayer.totalscre < _currentPlayer._scoreplayer.totalscre)
        {
            CheckState("win");
            //botwinner dont get next one 
        }
        else if (_currentAi._scoreplayer.totalscre > _currentPlayer._scoreplayer.totalscre)
        {

            CheckState("lose");
            //playerwinnget next
        }
        else
        {
            CheckState("draw");
            //draw go next
        }



    }
    private void CheckState(string state)
    {
        if (state == "win")
        {
            if (PlayerPrefs.GetInt("ai") < 7)
            {
                if (_currentPlayer._retryButton.activeInHierarchy) { _currentPlayer._retryButton.SetActive(false); }
                _currentPlayer._saveCbutt.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_currentPlayer._saveCbutt);
            }
            else
            {

                if (PlayerPrefs.HasKey("ai"))
                {
                    PlayerPrefs.DeleteKey("ai");
                }
                if (PlayerPrefs.HasKey("selectedai"))
                {
                    PlayerPrefs.DeleteKey("selectedai");
                }
                if (PlayerPrefs.HasKey("selectedplayerindx"))
                {
                    PlayerPrefs.DeleteKey("selectedplayerindx");
                }
                _currentPlayer.ShowRankedResult("arcade");

                if (!PlayerPrefs.HasKey("isaiah"))
                {
                    _currentPlayer._arcadereward.SetActive(true);
                    MainMenuAndNetworkManager.UnlouchAchivment("isaiah", 0);
                }
            }
        }
        else
        {
            if (_currentPlayer._saveCbutt.activeInHierarchy)
            {
                _currentPlayer._saveCbutt.SetActive(false);
            }
           
            _currentPlayer._retryButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_currentPlayer._retryButton);
        }
    }
    private void ApplyNextAiButt()
    {
        if (!_clicked)
        {

            if (PlayerPrefs.GetInt("ai") < 7)
            {

                var lastai = PlayerPrefs.GetInt("ai", 0);
                PlayerPrefs.SetInt("ai", lastai + 1);
                PlayerPrefs.Save();
                GetAiNameDependOnIndex(PlayerPrefs.GetInt("ai"));
                if (PlayerPrefs.GetInt("ai") == 7)
                {

                    SceneManager.LoadScene("battleRoyalScene");
                }
                else { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
                _clicked = true;
            }
         
        }
    }
  
    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                PlayerPrefs.SetString("selectedai", "AiMrBill");
                break;

        }
        PlayerPrefs.Save();
    }

   IEnumerator RunArcadeGame()
    {
        _vsPanel.SetActive(true);
      
        _previewbotImage.sprite = _bowlerSpritesAi[PlayerPrefs.GetInt("ai", 0)];
        _previewplayerImage.sprite = _bowlerSprites[PlayerPrefs.GetInt("selectedplayerindx",0)];
     
        yield return new WaitForSeconds(3f);

        _vsPanel.SetActive(false);
        PhotonNetwork.Instantiate("PhotonNetworkAvatar", transform.position, transform.rotation, 0);
        PhotonNetwork.Instantiate("PhotonAiAvatar", _spawnPoint.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        _currentAi = GameObject.FindGameObjectWithTag("Ai").GetComponent<AiController>();
        _currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _currentPlayer._saveCbutt.GetComponent<Button>().onClick.AddListener(ApplyNextAiButt);
        _currentPlayer._retryButton.GetComponent<Button>().onClick.AddListener(Retry);
    }

    
}
