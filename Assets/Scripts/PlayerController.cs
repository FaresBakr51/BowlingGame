using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;



public class PlayerController : MonoBehaviour
{

    public GameObject _ball;
    public Transform _playerhand;
    public Vector3 _BallConstantPos;
    public float _power;
    public GameObject _camera;
    private Animator _playerAnim;
    public GameObject _listpins;
    public GameObject _framestextobj;
    public GameObject _framesscoretextobj;
    public Slider _powerSlider;
    public Scrollbar _hookScroll;
    public List<Transform> _mypins = new List<Transform>();
    public List<Transform> _resetpins = new List<Transform>();
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
    public int[] roundScore;
    private int SumScore1;
    public int _roundscore;
    public int round;
    private ScorePlayer _scoreplayer;
    public BallSound _ballsound;
    public AudioClip _movingclip;
    public Text _totaleScoreTxt;
    public Text _playername;
    private void Awake()
    {
        GetReady();
        _canhit = true;
        _myxpos = transform.position.x;
        for (int i = 0; i < roundScore.Length; i++)
        {
            roundScore[i] = 0;
        }
        _scoreplayer = GetComponent<ScorePlayer>();
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.waiting, Waitstate);
        GameEventBus.Subscribe(GameEventType.leaderboard, CheckOtherHit);
    }
    private void OnDisable()
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
        _playername.text = GetNickname.nickname;
    }
    void Update()
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
       
        foreach (Transform obj in _listpins.GetComponentInChildren<Transform>())
        {
            _resetpins.Add(obj);

        }
        var pin = Instantiate(_listpins);
        foreach (Transform obj in pin.GetComponentInChildren<Transform>())
        {
            _mypins.Add(obj);
           
        }
       
        //foreach (Transform framtxt in _framestextobj.GetComponentInChildren<Transform>())
        //{
        //    _framesText.Add(framtxt.gameObject.GetComponent<TextMeshProUGUI>());

        //}
        //foreach (Transform framtxtscore in _framesscoretextobj.GetComponentInChildren<Transform>())
        //{
        //   _framescoretext.Add(framtxtscore.gameObject.GetComponent<TextMeshProUGUI>());

        //}
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

        roundCompleted();
    }
    

    private void ResetPins()
    {
        if(round % 2 != 0){
            for (int y = 0; y < _resetpins.Count; y++)
            {
                if (_mypins[y].gameObject.activeInHierarchy == false)
                {
                    _mypins[y].gameObject.SetActive(true);
                }
                _mypins[y + 1 - 1].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                _mypins[y + 1 - 1].transform.position = _resetpins[y + 1 - 1].transform.position;
                _mypins[y + 1 - 1].transform.rotation = _resetpins[y + 1 - 1].transform.rotation;

            }
        }
    }
    public void roundCompleted()
    {

      
        gaming(_roundscore);

        if (_roundscore == 10)
        {
            
            increase_round();
            gaming(0);

        }

        ResetPins();
    }

    public int round_number()
    {

        return round;

    }

    public void increase_round()
    {
        round++;
    }
    public void gaming(int score)
    {

        round = round_number();

        if (round < 19)
        {
            roundScore[round] = score;
            _scoreplayer.ScoreDisplay(roundScore);
            if (round % 2 == 1)
            {
                _scoreplayer.RoundScoreDisplay(roundScore);
            }

        }
        else if (round == 19)
        {
            roundScore[round] = score;
            _scoreplayer.ScoreDisplay(roundScore);
            _scoreplayer.RoundScoreDisplay(roundScore);

            SumScore1 = _scoreplayer.ScoreSum_Player1();
            Debug.Log("s1" + SumScore1);
            Invoke("SumFunction", 1);


        }

    }
    public void SumFunction()
    {
        _totaleScoreTxt.text = SumScore1.ToString();
        GameManager.instance.RequestLeaderBoard();
        

    }
}
