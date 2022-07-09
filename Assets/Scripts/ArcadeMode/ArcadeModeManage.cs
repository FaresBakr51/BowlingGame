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
     
        if(_currentAi._scoreplayer.totalscre > _currentPlayer._scoreplayer.totalscre)
        {
            _currentPlayer._saveCbutt.SetActive(false);
            _currentPlayer._GoHomebutt.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_currentPlayer._GoHomebutt);
            //botwinner dont get next one 
        }
        else if(_currentAi._scoreplayer.totalscre < _currentPlayer._scoreplayer.totalscre)
        {
         
            _currentPlayer._saveCbutt.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_currentPlayer._saveCbutt);
            //playerwinnget next
        }
        else
        {
            _currentPlayer._saveCbutt.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_currentPlayer._saveCbutt);
            //draw go next
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
                StartCoroutine(GetNextAiGame());
                _clicked = true;
            }
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
                PlayerPrefs.SetString("selectedai", "AiMrBill");
                break;

        }
        PlayerPrefs.Save();
    }
    IEnumerator GetNextAiGame()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        _currentPlayer._saveCbutt.GetComponent<Button>().onClick.AddListener(ApplyNextAiButt);
    }

    
}
