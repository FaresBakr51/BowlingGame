using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public GameObject _hookScrollobj;
    public GameObject _powersliderobj;
    public List<Transform> _mypins = new List<Transform>();
    //public List<GameObject> _nothitpins = new List<GameObject>();
    public List<Transform> _resetpins = new List<Transform>();
    public List<TextMeshProUGUI> _framesText = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> _framescoretext = new List<TextMeshProUGUI>();
    public int _score;
    public float _speed;
    private PlayerStateContext _playercontext;
    private PlayerState _waitingState, _BowlingState, _idleState;
    public float _slidertime;
    public float _scrolltime;
    private float inputdir;
    public GameObject _MyLeaderBoardCanavas;
    public GameObject _MyPlayCanavas;
    private float _myxpos;
    public bool _hookcalclated;
    public float _driftvalue;
    [SerializeField] private float _driftmaxvalu;
    public bool _canhit;
  

    private void Awake()
    {
        GetReady();
        _canhit = true;
        _myxpos = transform.position.x;
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.bowling, BowlState);
        GameEventBus.Subscribe(GameEventType.reset, IdleState);
        GameEventBus.Subscribe(GameEventType.waiting, Waitstate);
    }
    private void OnDisable()
    {
        GameEventBus.UnSubscribe(GameEventType.bowling, BowlState);
        GameEventBus.UnSubscribe(GameEventType.reset, IdleState);
        GameEventBus.UnSubscribe(GameEventType.waiting, Waitstate);
    }
    void Start()
    {

        _playercontext = new PlayerStateContext(this);
        _BowlingState = gameObject.AddComponent<PlayerBowlingState>();
        _waitingState = gameObject.AddComponent<PlayerWaitingState>();
        _idleState = gameObject.AddComponent<PlayerIdleState>();
        _playerAnim = GetComponent<Animator>();
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
                if ((Input.GetButton("xbutton") || Input.GetKey(KeyCode.Space)))
                {
                    UpdateGui();
                }
                if (Input.GetButtonUp("xbutton") || Input.GetKeyUp(KeyCode.Space))
                {
                _hookcalclated = true;
                GetDriftValue();
                GameEventBus.Publish(GameEventType.bowling);
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
       
        foreach (Transform framtxt in _framestextobj.GetComponentInChildren<Transform>())
        {
            _framesText.Add(framtxt.gameObject.GetComponent<TextMeshProUGUI>());

        }
        foreach (Transform framtxtscore in _framesscoretextobj.GetComponentInChildren<Transform>())
        {
           _framescoretext.Add(framtxtscore.gameObject.GetComponent<TextMeshProUGUI>());

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
        

        if (Input.GetButton("obutton") || Input.GetKey(KeyCode.O))
        {
            _scrolltime = 0;
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 1f, _scrolltime / 0.1f);

        }
        if (Input.GetButton("trianglebutton") || Input.GetKey(KeyCode.U)) {
            _scrolltime = 0;
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 0f, _scrolltime / 0.1f);
        }


    }
    private void GetDriftValue()
    {
        if (_hookScroll.value < 0.45f)
        {
            float driftval = (-_hookScroll.value * -_driftmaxvalu) - _driftmaxvalu;
            _driftvalue = driftval;
        }
        if (_hookScroll.value >= 0.45f)
        {
            float driftval = (_hookScroll.value * _driftmaxvalu);
            _driftvalue = driftval;
        }
    }
}
