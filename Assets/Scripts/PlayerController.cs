using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{

    public GameObject _ball;
    public Transform _playerhand;
    public Vector3 _BallConstantPos;
    public float _power;
    public GameObject _camera;
    private Animator _playerAnim;
    public GameObject _listpins;
    public Slider _powerSlider;
    public Scrollbar _hookScroll;
    public List<Transform> _mypins = new List<Transform>();
    private  List<Vector3> _resetpins = new List<Vector3>();
    private List<Quaternion> _resetpinsrot = new List<Quaternion>();
    public float _speed;
    private PlayerStateContext _playercontext;
    private PlayerState _waitingState, _BowlingState, _idleState;
    public float _slidertime;
    public float _scrolltime;
    private float inputdir;
    public GameObject _MyPlayCanavas;
    private float _myxpos;
    public bool _hookcalclated;
    public float _driftvalue;
    [SerializeField] private float _driftmaxvalu;
    public bool _canhit;
    public int _roundscore;
    private ScorePlayer _scoreplayer;
    public BallSound _ballsound;
    public AudioClip _movingclip;
    public TextMeshProUGUI _playername;
    private List<int> rolls = new List<int>();
    public bool _gameend;
   public GameObject _leaderboardprefab;
  //  public GameObject _myleaderboard;
    private GameObject _frametextobj;
    private GameObject _framescoretextobj;
    private PhotonView _photonview;
    public GameObject myleader;
    private GameObject _golballeaderboradcanavas;
    public GameObject _mypinsobj;
    // private const Vector3[] _pinpos;
    private void Awake()
    {
        _photonview = GetComponent<PhotonView>();
       
        _mypinsobj.transform.parent = null;
        _golballeaderboradcanavas = GameObject.FindWithTag("leaderboard");
        if (_photonview.IsMine)
        {
           
            myleader = PhotonNetwork.Instantiate("Panel", _leaderboardprefab.transform.position, _leaderboardprefab.transform.rotation);
           
         //   _mypinsobj = PhotonNetwork.Instantiate("pins", new Vector3(transform.position.x - 1.65f, _listpins.transform.position.y, _listpins.transform.position.z), _listpins.transform.rotation);
            myleader.transform.parent = _golballeaderboradcanavas.transform;
            myleader.gameObject.SetActive(false);
            myleader.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100f);
            myleader.transform.GetComponent<RectTransform>().localScale = new Vector3(2, 2f, 2);
        }
       
      
        if (!_photonview.IsMine)
        {
          _camera.GetComponent<Camera>().enabled = false;
          _camera.GetComponent<AudioListener>().enabled = false;
           Destroy(GetComponent<PlayerController>());
         //   Destroy(_ball.GetComponent<BallSound>());
        }
      
        _scoreplayer = GetComponent<ScorePlayer>();
        _canhit = true;
        _myxpos = transform.position.x;
        GetReady();
    }
    public override void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.waiting, Waitstate);
        GameEventBus.Subscribe(GameEventType.leaderboard, CheckOtherHit);
    }

    public override void OnDisable()
    {
        GameEventBus.UnSubscribe(GameEventType.leaderboard, CheckOtherHit);
        GameEventBus.UnSubscribe(GameEventType.waiting, Waitstate);
    }

    void Start()
    {
      
        _playercontext = new PlayerStateContext(this);
        _BowlingState = gameObject.AddComponent<PlayerBowlingState>();
        _waitingState = gameObject.AddComponent<PlayerWaitingState>();
        _idleState = gameObject.AddComponent<PlayerIdleState>();
        _playerAnim = GetComponent<Animator>();
        StartCoroutine(getmyname());
    }

    IEnumerator getmyname()
    {
        yield return new WaitForSeconds(2f);
      //  _playername.text = GetNickname.nickname;
    }
    void Update()
    {
        if (_photonview.IsMine)
        {
            if (_canhit == true)
            {
                inputdir = Input.GetAxis("Horizontal") * Time.deltaTime;
                transform.Translate(inputdir, 0, 0f);
                Vector3 clampedPosition = transform.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, _myxpos - 0.4f, _myxpos + 0.4f);
                transform.position = clampedPosition;

                if (_hookcalclated == false)
                {
                    UpdateHookSlider();
                }
                if (Input.GetButton("xbutton"))
                {
                    UpdateGui();
                }
                if (Input.GetButtonUp("xbutton"))
                {
                    _hookcalclated = true;
                    GetDriftValue();
                    BowlState();
                }
            }

        }
    }

    private void BowlState()
    {
        _playercontext.Transition(_BowlingState);
    }
    private void Waitstate()
    {
        _playercontext.Transition(_waitingState);
    }
    private void IdleState()
    {
        _playercontext.Transition(_idleState);
    }

    private void GetReady()
    {
        if (photonView.IsMine)
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
                _resetpins.Add(obj.transform.position);
                _resetpinsrot.Add(obj.transform.rotation);
            }

            foreach (Transform obj in _mypinsobj.GetComponentInChildren<Transform>())
            {
              
                _mypins.Add(obj);
            }

           
          
            StartCoroutine(waitReady());
        }

    }
 
    IEnumerator waitReady()
    {
        yield return new WaitForSeconds(1f);
        foreach (Transform framtxt in _frametextobj.GetComponentInChildren<Transform>())
        {
            _scoreplayer.scores_text.Add(framtxt.gameObject.GetComponent<Text>());

        }
        foreach (Transform framtxtscore in _framescoretextobj.GetComponentInChildren<Transform>())
        {
            _scoreplayer.round_scores_text.Add(framtxtscore.gameObject.GetComponent<Text>());

        }
    }
    public void UpdateAnimator(string val, int value)
    {
        _playerAnim.SetInteger(val, value);
    }
    private void UpdateGui()
    {
        _slidertime += Time.deltaTime;
        _powerSlider.value = Mathf.Lerp(_powerSlider.minValue, _powerSlider.maxValue, _slidertime / 2);
        _power = _powerSlider.value;
    }
    private void UpdateHookSlider()
    {
        if (Input.GetButton("obutton"))
        {
            _scrolltime = 0;
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 1f, _scrolltime / 0.6f);

        }
        if (Input.GetButton("trianglebutton")) {
            _scrolltime = 0;
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 0f, _scrolltime / 0.6f);
        }
    }
    private void GetDriftValue()
    {
        if (_hookScroll.value < 0.45f)
        {
            float driftval = (-_hookScroll.value * _driftmaxvalu);
            _driftvalue = driftval;
        }
        if (_hookScroll.value >= 0.45f)
        {
            float driftval = (_hookScroll.value * _driftmaxvalu);
            _driftvalue = driftval;
        }
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
        Debug.Log("reseted");
            for (int y = 0; y < _resetpins.Count; y++)
            {
                if (_mypins[y].gameObject.activeInHierarchy == false)
                {
                    _mypins[y].gameObject.SetActive(true);
                }
                _mypins[y + 1 - 1].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                _mypins[y + 1 - 1].transform.position = _resetpins[y + 1 - 1];
                _mypins[y + 1 - 1].transform.rotation = _resetpinsrot[y + 1 - 1];

            }
        
    }
    public void Bowl(int pinFall)
    {
        try
        {
            rolls.Add(pinFall);
            //SumScore1 += pinFall;
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

        
    }
   
    //public void SumFunction()
    //{
    //    _gameend = true;
    //    GameManager.instance.RequestFinalLeaderBoarD();


    //}

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
            _gameend = true;
            throw new UnityException("Don't know how to handle end game yet");
        }
    }

  
}

public class ActionMasterOld
{
    public enum Action { Tidy, Reset, EndTurn, EndGame };

    private int[] bowls = new int[21];
    private int bowl = 1;

    public static Action NextAction(List<int> pinfFalls)
    {
        ActionMasterOld am = new ActionMasterOld();
        Action currentAction = new Action();

        foreach (int pinFall in pinfFalls)
        {
            currentAction = am.Bowl(pinFall);
        }

        return currentAction;
    }

    private Action Bowl(int pins)
    { // TODO make private
        if (pins < 0 || pins > 10) { throw new UnityException("Invalid pins"); }

        bowls[bowl - 1] = pins;

        if (bowl == 21)
        {
            return Action.EndGame;
        }

        // Handle last-frame special cases
        if (bowl >= 19 && pins == 10)
        {
            bowl++;
            return Action.Reset;
        }
        else if (bowl == 20)
        {
            bowl++;
            if (bowls[19 - 1] == 10 && bowls[20 - 1] == 0)
            {
                return Action.Tidy;
            }
            else if (bowls[19 - 1] + bowls[20 - 1] == 10)
            {
                return Action.Reset;
            }
            else if (Bowl21Awarded())
            {
                return Action.Tidy;
            }
            else
            {
                return Action.EndGame;
            }
        }

        if (bowl % 2 != 0)
        { // First bowl of frame
            if (pins == 10)
            {
                bowl += 2;
                return Action.EndTurn;
            }
            else
            {
                bowl += 1;
                return Action.Tidy;
            }
        }
        else if (bowl % 2 == 0)
        { // Second bowl of frame
            bowl += 1;
            return Action.EndTurn;
        }

        throw new UnityException("Not sure what action to return!");
    }

    private bool Bowl21Awarded()
    {
        // Remember that arrays start counting at 0
        return (bowls[19 - 1] + bowls[20 - 1] >= 10);
    }
}
public static class ScoreMaster
{

    // Returns a list of cumulative scores, like a normal score card.
    public static List<int> ScoreCumulative(List<int> rolls)
    {
        List<int> cumulativeScores = new List<int>();
        int runningTotal = 0;

        foreach (int frameScore in ScoreFrames(rolls))
        {
            runningTotal += frameScore;
            cumulativeScores.Add(runningTotal);
        }

        return cumulativeScores;
    }

    // Return a list of individual frame scores.
    public static List<int> ScoreFrames(List<int> rolls)
    {
        List<int> frames = new List<int>();

        // Index i points to 2nd bowl of frame
        for (int i = 1; i < rolls.Count; i += 2)
        {
            if (frames.Count == 10) { break; }              // Prevents 11th frame score

            if (rolls[i - 1] + rolls[i] < 10)
            {               // Normal "OPEN" frame
                frames.Add(rolls[i - 1] + rolls[i]);
            }

            if (rolls.Count - i <= 1) { break; }                // Ensure at least 1 look-ahead available

            if (rolls[i - 1] == 10)
            {
                i--;                                        // STRIKE frame has just one bowl
                frames.Add(10 + rolls[i + 1] + rolls[i + 2]);
            }
            else if (rolls[i - 1] + rolls[i] == 10)
            {       // SPARE bonus
                frames.Add(10 + rolls[i + 1]);
            }
        }
        return frames;
    }
}

