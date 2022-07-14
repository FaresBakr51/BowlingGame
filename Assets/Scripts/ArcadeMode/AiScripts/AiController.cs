using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using UnityEngine.SceneManagement;
using BigRookGames.Weapons;

public class AiController : MonoBehaviour
{
    public GameObject _ball;
    public Transform _playerhand;
    public Vector3 _BallConstantPos;
 
    private Animator _playerAnim;
    public List<Transform> _mypins = new List<Transform>();
    [SerializeField] public List<Vector3> _resetpins = new List<Vector3>();
    [SerializeField] public List<Transform> _leftpins = new List<Transform>();

    private List<Quaternion> _resetpinsrot = new List<Quaternion>();
    public float _speed;
  
    private AiStates _waitingState, _bowlingState, _resetState;
    //public float _slidertime;
    //public float _scrolltime;
  //  public GameObject _MyPlayCanavas;
    public bool _hookcalclated;
    public bool _canhit;
    public int _roundscore;
    public ScorePlayer _scoreplayer;
    public AudioClip _movingclip;
    private List<int> rolls = new List<int>();
    public bool _gameend;
    public GameObject _leaderboardprefab;
    private GameObject _frametextobj;
    private GameObject _framescoretextobj;
    private PhotonView _photonview;
    public GameObject myleader;
    private GameObject _golballeaderboradcanavas;
    public GameObject _mypinsobj;
    public bool _followBall;
    public GameObject _mytotal;
    public GameControls _gameactions;
    public bool _powerval;
    //private bool _moveright;
    //private Vector3 _movingL;

    public bool _calcScore;
    //   public bool _calcPower;
    //  private bool _ControllPower;
    public bool _checkIfthereOther;
    [Header("ArcadeGame")]
    //public bool _canPlay;
    public int shotType;




   
    [Header("PhotonaAvatarAndVoiceManager")]
 
    public GameObject _myManager;

    [Header("SlidersProp")]


    public float powerminval;
    public float powermaxval;
    public float _power;

   
    public float _driftvalue;
    public float _drifMinval;
    public float _driftMaxval;
    [Header("SpecialWeponProp")]
    public GameObject _myRocket;
    public bool _usingRock;
    public bool _usedRocket;
    public bool _readyLunch;
    public bool _infiniteRockets;
    private Vector3 _mypos;
    void Awake()
    {

        shotType = Random.Range(0, 2);
        _mypos = this.transform.position;
        _photonview = GetComponent<PhotonView>();
        _mypinsobj.transform.parent = null;
        if (SceneManager.GetActiveScene().name == "Map4")
        {
            _mypinsobj.transform.position = new Vector3(_mypinsobj.transform.position.x, _mypinsobj.transform.position.y, _mypinsobj.transform.position.z - 0.5f);
        }
        else
        {
            _mypinsobj.transform.position = new Vector3(_mypinsobj.transform.position.x, _mypinsobj.transform.position.y, _mypinsobj.transform.position.z + 0.3f);
        }
        _golballeaderboradcanavas = GameObject.FindWithTag("leaderboard");
        _scoreplayer = GetComponent<ScorePlayer>();
        if (!_photonview.IsMine)
        {
            //_camera.GetComponent<Camera>().enabled = false;
            //_camera.GetComponent<AudioListener>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
         

        }
        
        _canhit = true;
    
    }
 
    private void Start()
    {


      
        if (_photonview.IsMine)
        {

            if (!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom))
            {
                myleader = PhotonNetwork.Instantiate("Panel", _leaderboardprefab.transform.position, _leaderboardprefab.transform.rotation);
            }
       
       //     var rankedpanelobj = myleader.GetComponentsInChildren<Transform>();
      //      var playcanavas = _MyPlayCanavas.GetComponentsInChildren<Transform>().ToList();

            myleader.GetComponentInChildren<Button>().gameObject.SetActive(false);
            myleader.transform.parent = _golballeaderboradcanavas.transform;
            myleader.gameObject.SetActive(false);
            myleader.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100f);
            myleader.transform.GetComponent<RectTransform>().localScale = new Vector3(2, 2f, 2);
        }
     
        _bowlingState = gameObject.AddComponent<AiBowlState>();
        _waitingState = gameObject.AddComponent<AiWaitingState>();
        _resetState = gameObject.AddComponent<AiResetState>();
        _playerAnim = GetComponent<Animator>();
        GetReady();
        CheckShotType();


    }
    public void RunRpc()
    {
        if (_photonview.IsMine)
        {
            _photonview.RPC("RPCRocketOFF", RpcTarget.All);
        }
    }
    [PunRPC]
    private void RPCHiRocket()
    {
        _myRocket?.SetActive(true);
        transform.rotation = Quaternion.Euler(transform.rotation.x, 200, transform.rotation.z);
        _ball?.SetActive(false);
       
    }
    [PunRPC]
    private void RPCRocketOFF()
    {

        _myRocket.SetActive(false);
        _ball.SetActive(true);
    }
    public void CheckShotType()
    {
        shotType = Random.Range(0, 2);
        if (shotType == 1)
        {
            if (!_usedRocket)
            {
                _usingRock = true;
                transform.position = _mypos;
                UpdateAnimator("shot", 2);

                if (_photonview.IsMine)
                {
                    _photonview.RPC("RPCHiRocket", RpcTarget.All);
                }
                StartCoroutine(readyLunch());
                _usedRocket = true;
            }
        }
    }
    IEnumerator readyLunch()
    {
        yield return new WaitForSeconds(2);
        _readyLunch = true;
        yield return new WaitForSeconds(1.5f);

        foreach (Transform pin in _mypins)
        {
            pin.transform.rotation = Quaternion.Euler(pin.rotation.x, pin.rotation.y, Random.Range(90, 180));

        }

    }
    void Update()
    {
        if (_photonview.IsMine)
        {
            
            if (_canhit)
            {
                if (_readyLunch)
                {
                    _myRocket?.GetComponent<GunfireController>().FireWeapon();
                    Waitstate();
                    _readyLunch = false;

                }
                if (!_usingRock)
                {
                    if (!_hookcalclated)
                    {

                        UpdateHookSlider();
                    }
                    if (_hookcalclated && !_powerval)
                    {

                        UpdateGui();
                    }
                }

            }

        }
    }
    private void GetReady()
    {

        if (_photonview.IsMine)
        {
            foreach (Transform obj in myleader.GetComponentInChildren<Transform>())
            {

                if (obj.name == "frametextobj")
                {
                    _frametextobj = obj.gameObject;
                }
                if (obj.name == "framescoretextobj")
                {
                    _framescoretextobj = obj.gameObject;
                }
            }
            foreach (Transform obj in _mypinsobj.GetComponentInChildren<Transform>())
            {

                _mypins.Add(obj);
                _resetpinsrot.Add(obj.transform.rotation);
            }

            StartCoroutine(waitReady());
        }

    }
    IEnumerator waitReady()
    {
        ResetPins();
        foreach (Transform pins in _mypins)
        {
            if (pins.name != "PinSetter")
            {
                pins.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            }
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform pins in _mypins)
        {
            if (pins.name != "PinSetter")
            {
                _resetpins.Add(pins.localPosition);

            }
        }
        //if (GameModes._rankedMode || GameModes._battleRoyale)
        //{
        //    _modePlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
        //    if (_modePlayers.Contains(this.gameObject))
        //    {
        //        _modePlayers.Remove(this.gameObject);
        //    }
        //}


        foreach (Transform framtxt in _frametextobj.GetComponentInChildren<Transform>())
        {
            _scoreplayer.scores_text.Add(framtxt.gameObject.GetComponent<Text>());

        }
        foreach (Transform framtxtscore in _framescoretextobj.GetComponentInChildren<Transform>())
        {
            _scoreplayer.round_scores_text.Add(framtxtscore.gameObject.GetComponent<Text>());

        }
        _checkIfthereOther = true;
    }
    public void UpdateAnimator(string val, int value)
    {
        _playerAnim.SetInteger(val, value);
    }
    private void UpdateGui()
    {

        GetPowerValue();

    }
 
    private void GetPowerValue()
    {
        _powerval = true;
        _power = Random.Range(powerminval,powermaxval);
         BowlState();
    }
    private void UpdateHookSlider()
    {
        _driftvalue = Random.Range(_drifMinval, _driftMaxval);
        _hookcalclated = true;
      
    }

 
 
    private void BowlState()
    {
        if (!_gameend)
        {
            TranslateState(_bowlingState);

        }
    }

    public void Waitstate()
    {
        TranslateState(_waitingState);
    }

    public void IdleState()
    {
        TranslateState(_resetState);
      
    }


    public void CheckOtherHit()
    {


        AddToLeaderBoard();
        IdleState();
    }

    private void AddToLeaderBoard()
    {
        Bowl(_roundscore);
    }
    private void ResetPins()
    {

        if (_photonview.IsMine)
        {
            _photonview.RPC("ResetPinsRunCounterAi", RpcTarget.All);
        }


    }
    [PunRPC]
    private void ResetPinsRunCounterAi()
    {
        for (int y = 0; y < _resetpins.Count; y++)
        {
            if (_mypins[y].gameObject.activeInHierarchy == false)
            {
                _mypins[y].gameObject.SetActive(true);
            }
            if (_mypins[y].name != "PinSetter")
            {
                _mypins[y].gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
                _mypins[y + 1 - 1].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                _mypins[y + 1 - 1].transform.localPosition = _resetpins[y + 1 - 1];
                _mypins[y + 1 - 1].transform.rotation = _resetpinsrot[y + 1 - 1];
            }

        }
        _leftpins.Clear();
    }
    public void Bowl(int pinFall)
    {
       
        try
        {
            rolls.Add(pinFall);
            PerformAction(ActionMasterOld.NextAction(rolls));
        }
        catch
        {
            Debug.LogWarning("Something went wrong in Bowl()");
        }

        try
        {
            _scoreplayer.FillRolls(rolls);
            _scoreplayer.FillFrames(ScoreMaster.ScoreCumulative(rolls));
        }
        catch
        {
            Debug.LogWarning("FillRollCard failed");
        }
        _calcScore = true;
    }
    public void PerformAction(ActionMasterOld.Action action)
    {
        if (action == ActionMasterOld.Action.EndTurn)
        {

            ResetPins();
        }
        else if (action == ActionMasterOld.Action.Reset)
        {

            ResetPins();
        }
        else if (action == ActionMasterOld.Action.EndGame)
        {

            GameFinished();
            throw new UnityException("Don't know how to handle end game yet");
        }
    }
    public void TranslateState(AiStates _state)
    {
        _state.ProcessState(this);
    }
    private void GameFinished()
    {
        _gameend = true;
    }
}
