using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using UnityEngine.SceneManagement;
public class AiController : MonoBehaviour
{
    public GameObject _ball;
    public Transform _playerhand;
    public Vector3 _BallConstantPos;
    public float _power;
   // public GameObject _camera;
    private Animator _playerAnim;
    public Slider _powerSlider;
    public Scrollbar _hookScroll;
    public List<Transform> _mypins = new List<Transform>();
    [SerializeField] public List<Vector3> _resetpins = new List<Vector3>();
    [SerializeField] public List<Transform> _leftpins = new List<Transform>();

    private List<Quaternion> _resetpinsrot = new List<Quaternion>();
    public float _speed;
  
    private AiStates _waitingState, _bowlingState, _resetState;
    public float _slidertime;
    public float _scrolltime;
    private Vector3 inputdir;
    public GameObject _MyPlayCanavas;
    private float _myxpos;
    public bool _hookcalclated;
    public float _driftvalue;
    [SerializeField] private float _driftMaxval;
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
    public GameObject _GoHomebutt;
    public GameControls _gameactions;
    public bool _powerval;
    private bool _moveright;
    private Vector3 _movingL;

    public bool _calcScore;
    public bool _calcPower;
    private bool _ControllPower;

    public bool _checkIfthereOther;
    //[SerializeField] public GameObject _myRocket;
    //[SerializeField] public GameObject _RocketOff;
    //[SerializeField] public GameObject _RocketOn;
   // public bool _usedRocket;
    //public bool _readyLunch;
    //public List<GameObject> _modePlayers;


   // public bool _usingRock;
    private Vector3 _mypos;
    void Awake()
    {
        _mypos = this.transform.position;
        _powerSlider.gameObject.SetActive(false);
        _hookScroll.gameObject.SetActive(false);
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
            Destroy(_MyPlayCanavas);

        }
        _canhit = true;
        _myxpos = transform.position.x;
    }
    private void Start()
    {
        if (_photonview.IsMine)
        {

            if (!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom))
            {
                myleader = PhotonNetwork.Instantiate("Panel", _leaderboardprefab.transform.position, _leaderboardprefab.transform.rotation);
            }
            _GoHomebutt = myleader.GetComponentInChildren<Button>().gameObject;
            var rankedpanelobj = myleader.GetComponentsInChildren<Transform>();
            var playcanavas = _MyPlayCanavas.GetComponentsInChildren<Transform>().ToList();

            myleader.GetComponentInChildren<Button>().gameObject.SetActive(false);
            myleader.transform.parent = _golballeaderboradcanavas.transform;
            myleader.gameObject.SetActive(false);
            myleader.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100f);
            myleader.transform.GetComponent<RectTransform>().localScale = new Vector3(2, 2f, 2);
        }
        _hookScroll.gameObject.SetActive(false);
        _powerSlider.gameObject.SetActive(false);
        _bowlingState = gameObject.AddComponent<AiBowlState>();
        _waitingState = gameObject.AddComponent<AiWaitingState>();
        _resetState = gameObject.AddComponent<AiResetState>();
        _playerAnim = GetComponent<Animator>();
        GetReady();
     
    }

    void Update()
    {
        if (_photonview.IsMine)
        {
          

            //if (_gameend)
            //{
            //    //if (_MyPlayCanavas.activeInHierarchy)
            //    //{
            //    //    _MyPlayCanavas.SetActive(false);
            //    //}
            //    //if (!_GoHomebutt.activeInHierarchy)
            //    //{
            //    //    _GoHomebutt.SetActive(true);
            //    //}
            //    //if (!myleader.activeInHierarchy)
            //    //{
            //    //    myleader.SetActive(true);
            //    //}
            //    //EventSystem.current.SetSelectedGameObject(_GoHomebutt);

            //}
            if (_canhit)
            {
            
                if (!_hookcalclated)
                {

                    UpdateHookSlider();
                }
                else
                {
                    _hookScroll.gameObject.SetActive(false);
                }
                if (_hookcalclated && !_powerval)
                {

                    UpdateGui();
                }
                //if (!_usingRock)
                //{
                //    if (!_gameend)
                //    {
                //        //inputdir = new Vector3(_movingL.x, 0, 0);
                //        //transform.Translate(inputdir * Time.deltaTime);
                //        //Vector3 clampedPosition = transform.position;
                //        //clampedPosition.x = Mathf.Clamp(clampedPosition.x, _myxpos - 0.4f, _myxpos + 0.4f);
                //        //transform.position = clampedPosition;
                //    }
                //}

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
        if (_powerSlider.value == _powerSlider.maxValue)
        {

            _ControllPower = true;
            _slidertime = 0;

        }
        if (_powerSlider.value == _powerSlider.minValue)
        {
            _slidertime = 0;

            _ControllPower = false;

        }

        if (_ControllPower == false)
        {
            _slidertime += Time.deltaTime;
            _powerSlider.value = Mathf.Lerp(_powerSlider.minValue, _powerSlider.maxValue, _slidertime / 0.3f);
        }
        else
        {

            _slidertime += Time.deltaTime;
            _powerSlider.value = Mathf.Lerp(_powerSlider.maxValue, _powerSlider.minValue, _slidertime / 0.3f);
        }


    }
    IEnumerator ActivePowerShot()
    {

        yield return new WaitForSeconds(0.5f);
        _calcPower = true;
    }
    private void GetPowerValue()
    {
        _powerval = true;
        _power = _powerSlider.value;
         BowlState();
    }
    private void UpdateHookSlider()
    {
        if (_moveright == false)
        {

            _scrolltime = 0;
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 1f, _scrolltime / 0.168f);

            if (_hookScroll.value >= 0.9)
            {
                _moveright = true;
            }
        }
        else
        {
            _scrolltime = 0;
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 0f, _scrolltime / 0.168f);//0.3

            if (_hookScroll.value <= 0.1f)
            {

                _moveright = false;
            }

        }

    }

    private void GetDriftValue()
    {
        if (_hookScroll.value == 0.45f)
        {
            _driftvalue = 0;
        }
        else
        {
            float driftval = (Mathf.Round(_hookScroll.value * 10));

            GetDrifRealVal(driftval);
        }
        _hookcalclated = true;
        _hookScroll.gameObject.SetActive(false);
        _powerSlider.gameObject.SetActive(true);
        StartCoroutine(ActivePowerShot());
    }
    private void GetDrifRealVal(float val)
    {

        switch (val)
        {
            case 0:
                _driftvalue = (_driftMaxval * -1);
                break;
            case 1:
                _driftvalue = (_driftMaxval * -1);//ex : -100
                break;
            case 2:
                _driftvalue = (_driftMaxval * -1) + 20;//ex -80
                break;
            case 3:
                _driftvalue = (_driftMaxval * -1) + 40;
                break;
            case 4:
                _driftvalue = (_driftMaxval * -1) + 60;
                break;
            case 5:
                _driftvalue = 0;
                break;
            case 6:
                _driftvalue = _driftMaxval - 60;
                break;
            case 7:
                _driftvalue = _driftMaxval - 40;
                break;
            case 8:
                _driftvalue = _driftMaxval - 20;
                break;
            case 9:
                _driftvalue = _driftMaxval;
                break;
            case 10:
                _driftvalue = _driftMaxval;
                break;


        }
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

    private void CheckOtherHit()
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
            _photonview.RPC("ResetPinsRunCounter", RpcTarget.All);
        }


    }
    [PunRPC]
    private void ResetPinsRunCounter()
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
