using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

//#if !UNITY_WEBGL
//using Photon.Voice.PUN;
//#endif
using UnityEngine.Networking;
using BigRookGames.Weapons;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Special;
using BackEnd;

public class PlayerController : MonoBehaviourPunCallbacks,IPunObservable
{

    public GameObject _ball;
    public Transform _playerhand;
    public Vector3 _BallConstantPos;
    public float _power;
    public GameObject _camera;
    private Animator _playerAnim;
    public Image _powerSliderImg;
    [SerializeField] private float minBowlPower;
    [SerializeField] private float maxBowlPower;
    public RectTransform _hookScrollRect;

    public Scrollbar spinScroll;
    [SerializeField] private float minHookX;
    [SerializeField] private float maxHookX;
    public List<Transform> _mypins = new List<Transform>();
    public GameObject myPinsSetter;
    [SerializeField] public  List<Vector3> _resetpins = new List<Vector3>();
    [SerializeField] public List<Transform> _leftpins = new List<Transform>();

    private List<Quaternion> _resetpinsrot = new List<Quaternion>();
    public float _speed;
    private PlayerStateContext _playercontext;
    private PlayerState _waitingState, _BowlingState, _idleState;
    public float _slidertime;
    public float hookDuration = 1;
    public float spinDuration = 0.3f;
    public float _scrolltime;
    public float _spinTime;
    private Vector3 inputdir;
    public GameObject _MyPlayCanavas;
    private float _myxpos;
    public bool _hookcalclated;
    public bool spinCalculated;
    public float _driftvalue;
    public float spinMinValue;
    public float spinMaxValue;
    public bool leftSpin;
    public float spinvalue;
    public float spinScaler;
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
    public GameObject _myPinsParent;
    public bool _followBall;
    public GameObject _mytotal;
    public GameObject _GoHomebutt;
    public GameControls _gameactions;
    public bool _powerval;
    [SerializeField] private bool _moveright;
    [SerializeField] private bool _moverightspin;
    [SerializeField] private Vector3 _movingL;
    [SerializeField] public FixedJoystick joyStick;
    [SerializeField] public FixedJoystick hookStick;
    public bool _calcScore;
    public bool _calcPower;
    private bool _ControllPower;
   
    public int _mycontroll;
    public AudioSource _gameAudio;
    public AudioClip[] _gameClips;
    public AudioClip[] _FramesClips;
    public GameObject _strikeTxt;
    public GameObject _spareTxt;
    public GameObject _gutterTxt;
    [SerializeField] private GameObject _pauseMenupanel;
    [SerializeField] private GameObject _pauseMenuFirstbutt;
    [SerializeField] private GameObject[] _soundOnOF;
    
    private bool _gamePaused;
    [Header("ArcadeGame")]
    public GameObject _saveCbutt;
    public bool publishGameEnd;
    public GameObject _retryButton;
    public GameObject _arcadereward;

    [Header("PhotonaAvatarAndVoiceManager")]
//    #if !UNITY_WEBGL
//    [SerializeField] private PhotonVoiceView _myVoice;
//#endif
    public GameObject _isspeakingButt;
    public GameObject _notSpeakingButt;
    public GameObject _myManager;

  
 
    
    public bool _checkIfthereOther;

    [Header("SpecialWeponProp")]
    [SerializeField] public GameObject _myRocket;
    [SerializeField] public GameObject _RocketOff;
    [SerializeField] public GameObject _RocketOn;
    public float fireballspeed;
    public AudioClip _fireballSoundClip;
    public bool usedAbility;
    public bool _readyLunch;
    public List<GameObject> _modePlayers;
    [SerializeField] private Transform wallPos;
    // [SerializeField] public GameObject _myFireBall;
   
    public bool _fireball;
    public bool _usingRock;
    public Vector3 _mypos;
    

    [Header("RankedMode")]
    public GameObject _rankedPanel;
    [SerializeField] private Text _rankedpointtxt;
    [SerializeField] private Text _rankedstatetxt;
    public bool _gameRankedFinished;
    public RankedModeState _rankedMode;
    public GameObject _waitOtherPlayer;

    [Header("BattleRoyal")]
    public float _timerAfk;
    public bool _battleStart;
    public float battletimer = 7;
    public GameObject _battleRoyalDescrypt;
    public RoyalModeState _royalMode;
    public int _trackScore;
    public int _checkCond;
    [Header("TrackBall")]
    [SerializeField] public bool _trackBall;
    public bool _waitTrackBall;
    [SerializeField] private GameObject[] _trackBallOnOf;
    public Image _trackBallImage;
    [SerializeField] private GameObject _progressPowerUi;
    public float presistencePower;
    public bool waitingPUSH;

    [Header("BallControll")]
    public bool _driftBall;
    [SerializeField] private float _controllBallPower;
    [Header("ScoreMotions")]

    public IDictionary<int,AnimationClip> _DanceClips = new Dictionary<int, AnimationClip>();
    public AnimationClip[] _danceClips;
    private int keys = 3;
    public bool _dance;




    [Header("TouchInputs")]
    public bool IGT;

    public Image _filledImage;
    private bool _startCheck;
    public float force;



    [SerializeField] private float minPower;
    [SerializeField] private float maxPower;
    private void Awake()
    {
      for(int i = 0; i < _danceClips.Length; i++)
        {

            _DanceClips.Add(keys, _danceClips[i]);
            keys++;
        }
        //  _myVoice = GetComponent<PhotonVoiceView>();
        _mypos = this.transform.position;

        _powerSliderImg.gameObject.SetActive(false);
        _hookScrollRect.gameObject.SetActive(false);
         _gameactions = new GameControls();
        _photonview = GetComponent<PhotonView>();
        _mypinsobj.transform.parent = null;
        _ball.GetComponent<TrailRenderer>().enabled = false;

        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            if (tr.name == "spawnWallPoint")//get player wall point
            {
                tr.parent = null;
                wallPos = tr;
              
                break;
            }
        }
        if (SceneManager.GetActiveScene().name == "battleRoyalScene")//OnBattleRoyal
        {
            _mypinsobj.transform.position = new Vector3(_mypinsobj.transform.position.x, _mypinsobj.transform.position.y, _mypinsobj.transform.position.z - 1f);
        }
        else
        {
            _mypinsobj.transform.position = new Vector3(_mypinsobj.transform.position.x, _mypinsobj.transform.position.y, _mypinsobj.transform.position.z -0.2f);
        }
        _golballeaderboradcanavas = GameObject.FindWithTag("leaderboard");
          _scoreplayer = GetComponent<ScorePlayer>();
        if (!_photonview.IsMine)
        {
          _camera.GetComponent<Camera>().enabled = false;
          _camera.GetComponent<AudioListener>().enabled = false;
         GetComponent<PlayerController>().enabled = false;
            Destroy(_MyPlayCanavas);

        }
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
       
         _canhit = true;
        _myxpos = transform.position.x;

    }
 
     public void UpdateSound(AudioClip clip){

     
        _gameAudio.PlayOneShot(clip);
        
    }
    public override void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.waiting, Waitstate);
        GameEventBus.Subscribe(GameEventType.leaderboard, CheckOtherHit);
        if (joyStick == null)
        {
            joyStick = GetComponentInChildren<FixedJoystick>();
        }
        if (!Application.isMobilePlatform)//disable mobile stuff
        {
            joyStick?.gameObject.SetActive(false);
            
        }
        _gameactions.ButtonActions.moving.performed += cntxt =>
        {
            OnHookAction(cntxt.ReadValue<Vector2>());
        };
          
       _gameactions.ButtonActions.moving.canceled += cntxt => _movingL = Vector2.zero;
        _gameactions.ButtonActions.pause.performed += x =>
        {

            if (!_gameend && !_usingRock && _battleStart)
            {
                Pause();
            }
        };
        _gameactions.ButtonActions.Rocket2.performed += r =>
        {

            LunchRocket();
        };

        _gameactions.Enable();
    }

    private void OnHookAction(Vector2 val)
    {
       
        if (!_gamePaused && !_pauseMenupanel.activeInHierarchy)
        {
        //    Debug.Log(val);
            _movingL = val;//cntxt.ReadValue<Vector2>();
            Debug.Log(_movingL);
        }
        if (!_driftBall) return;
      //  Debug.Log("Listening drift");
        if (_movingL.x > 0)
        {
            Debug.Log("left drift");
            var rig = _ball.GetComponent<Rigidbody>();
            rig.AddForce(new Vector3(-_controllBallPower, 0, 0), ForceMode.Impulse);
            _driftBall = false;

        }
        else if (_movingL.x < 0)
        {
            Debug.Log("right drift");
            var rig = _ball.GetComponent<Rigidbody>();
            rig.AddForce(new Vector3(_controllBallPower, 0, 0), ForceMode.Impulse);
            _driftBall = false;

        }

    }

    public void LunchRocket()
    {
        if (_canhit && !GameModes._battleRoyale)
        {
            if (!usedAbility)
            {

                if (!_gamePaused && _battleStart)
                {
                    //_usingRock = true;
                    //transform.position = _mypos;
                    //UpdateAnimator("shot", 2);

                    //if (_photonview.IsMine)
                    //{
                    //    _photonview.RPC("RPCHiRocket", RpcTarget.All);
                    //}
                    //StartCoroutine(readyLunch());
                    SpecialBase speical = GetComponent<SpecialBase>();
                    speical.SpawnAbility(photonView, wallPos);
                    usedAbility = true;
                }
            }
        }


    }
    public void RunRpcDance(bool state)
    {

        if (_photonview.IsMine)
        {
            _photonview.RPC("RPCDance", RpcTarget.All, state);
        }
    }
    [PunRPC]
    private void RPCDance(bool state)
    {
        _ball.SetActive(state);
    }
    public void RunRpc()
    {
        if (_photonview.IsMine)
        {
            _photonview.RPC("RPCRocketOFF", RpcTarget.All);
        }
    }
    public void HitRocket()
    {
        if (_photonview.IsMine)
        {
            _photonview.RPC("RPCHiRocket", RpcTarget.All);
        }
         StartCoroutine(readyLunch());
    }
    [PunRPC]
    public void RPCHiRocket()
    {
        _myRocket?.SetActive(true);
        if (!_fireball)
        {
            
            transform.rotation = Quaternion.Euler(transform.rotation.x, 200, transform.rotation.z);
        }
        _ball?.SetActive(false);
       
    }
    [PunRPC]
    private void RPCRocketOFF()
    {
        if (!_fireball)
        {
            _myRocket.SetActive(false);
        }
        _ball.SetActive(true);
    }
    IEnumerator readyLunch()
    {
        if (_fireball)
        {
            UpdateSound(_fireballSoundClip);
            yield return new WaitForSeconds(0.5f);
            _myRocket.transform.parent = null;
            _myRocket.transform.rotation = Quaternion.Euler(_myRocket.transform.rotation.x, -180, _myRocket.transform.rotation.z);
            _myRocket.transform.position = new Vector3(_myRocket.transform.position.x + 0.4f, _myRocket.transform.position.y - 0.7f, _myRocket.transform.position.z);
            _myRocket.GetComponent<ProjectileMover>().myPlayer = this;
            _myRocket.GetComponent<ProjectileMover>().speed = fireballspeed;
            _readyLunch = true;
        }
        else
        {
            yield return new WaitForSeconds(2);
            _readyLunch = true;
        }
         //yield return new WaitForSeconds(1.5f);

         // foreach (Transform pin in _mypins)
         // {
         //       pin.transform.rotation = Quaternion.Euler(pin.rotation.x, pin.rotation.y, Random.Range(90, 180));
                
         // }
        
    }
    public void GetAllPinsDown()
    {
        foreach (Transform pin in _mypins)
        {
            pin.transform.rotation = Quaternion.Euler(pin.rotation.x, pin.rotation.y, Random.Range(90, 180));

        }
        Waitstate();

    }
    public override void OnDisable()
    {
        GameEventBus.UnSubscribe(GameEventType.leaderboard, CheckOtherHit);
        GameEventBus.UnSubscribe(GameEventType.waiting, Waitstate);
          _gameactions.Disable();
       
    }
  
    void Start()
    {
     
            CheckControlles();
        
        if (_photonview.IsMine)
        {
            _timerAfk = 15;
            if (!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom))
            {
                myleader = PhotonNetwork.Instantiate("Panel", _leaderboardprefab.transform.position, _leaderboardprefab.transform.rotation);
            }
            var leadercomp = myleader.GetComponentsInChildren<Transform>();
            _GoHomebutt = leadercomp.FirstOrDefault(x=>x.name == "HomeButt").gameObject;
           
            
            _saveCbutt = leadercomp.FirstOrDefault(x => x.name == "save&continue").gameObject;
            _retryButton = leadercomp.FirstOrDefault(x => x.name == "retry").gameObject;
            _arcadereward = leadercomp.FirstOrDefault(x => x.name == "arcadereward").gameObject;
            _arcadereward.SetActive(false);
            _retryButton.SetActive(false);
            _saveCbutt.SetActive(false);
            _waitOtherPlayer = leadercomp.FirstOrDefault(x => x.name == "waitingotherplayer").gameObject;
            var playcanavas = _MyPlayCanavas.GetComponentsInChildren<Transform>().ToList();
         //   _battleRoyalDescrypt = playcanavas.FirstOrDefault(x => x.name == "BattleRoyalDescrypt").gameObject;
            //_battleRoyalDescrypt.SetActive(false);
            if (GameModes._battleRoyale)
            {
                _battleRoyalDescrypt.SetActive(true);
            }
            else { _battleStart = true; }
            _waitOtherPlayer.SetActive(false);
            _rankedPanel = leadercomp.FirstOrDefault(x => x.name == "RankedPanel").gameObject;
            var rankedpanobj = _rankedPanel.GetComponentsInChildren<Transform>();
            _rankedpointtxt = leadercomp.FirstOrDefault(x => x.name == "rankedpoints").GetComponent<Text>();
            _rankedstatetxt = leadercomp.FirstOrDefault(x => x.name == "rankedstate").GetComponent<Text>();
            _rankedPanel.SetActive(false);
            myleader.GetComponentInChildren<Button>().gameObject.SetActive(false);
            myleader.transform.parent = _golballeaderboradcanavas.transform;
            myleader.gameObject.SetActive(false);
            myleader.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100f);
            myleader.transform.GetComponent<RectTransform>().localScale = new Vector3(2, 2f, 2);


        }
        _hookScrollRect.gameObject.SetActive(true);
        _playercontext = new PlayerStateContext(this);
        _BowlingState = gameObject.AddComponent<PlayerBowlingState>();
        _waitingState = gameObject.AddComponent<PlayerWaitingState>();
        _idleState = gameObject.AddComponent<PlayerIdleState>();
        _playerAnim = GetComponent<Animator>();
         GetReady();
        if (GameModes._battleRoyale)
        {
            _royalMode = gameObject.AddComponent<RoyalModeState>();
            _royalMode.GameMode(this);
        }
        else if (GameModes._rankedMode)
        {
            _rankedMode = gameObject.AddComponent<RankedModeState>();
            _rankedMode.GameMode(this);
         //   StartCoroutine(TestRanked());
        }
      
    }
    //IEnumerator TestRanked()
    //{
    //    yield return new WaitForSeconds(4f);
    //    _scoreplayer.totalscre = Random.Range(100, 200);
    //    _gameend = true;
    //}

    private void CheckControlles(){

       
        if (!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode &&PhotonNetwork.InRoom )){

            //_gameactions.ButtonActions.trackBall.performed += e =>
            //{
            //    if (_trackBall)
            //    {

            //        if (!_gamePaused && !_pauseMenupanel.activeInHierarchy)
            //        {
            //            if (_canhit && _ball.activeInHierarchy && _battleStart)
            //            {

            //                if (_hookcalclated)
            //                {
            //                    if (_calcPower)
            //                    {

            //                      Vector2 v =  e.ReadValue<Vector2>();
            //                        if(v.y <= 0)
            //                        {
            //                            _power = _powerSlider.minValue ;
            //                        }else if(v.y == 1)
            //                        {
            //                            _power = _powerSlider.minValue + Mathf.Abs(v.y + (((_powerSlider.minValue - _powerSlider.maxValue) /2 ) -1));
            //                        }else if(v.y > 1)
            //                        {
            //                            _power = _powerSlider.maxValue ;
            //                        }

            //                        BowlState();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //};

            _gameactions.ButtonActions.powerupaction.performed += x => {

                OnPowerUpAction();
            };

            _gameactions.ButtonActions.driftbar.performed += y => {

                OnDriftAction();
            };

            if (leftSpin)
            {
                _gameactions.ButtonActions.SpinBarLeft.performed += y =>
                {

                    OnGetSpinValue();
                };
            }
            else
            {
                _gameactions.ButtonActions.SpinBarRight.performed += y =>
                {

                    OnGetSpinValue();
                };
            }
        }
        // #region IGT
        //_gameactions.ButtonActions.IGTpowerAction.performed += val =>
        //{


        //    if (!_gamePaused && !_pauseMenupanel.activeInHierarchy)
        //    {
        //        if (_canhit && _ball.activeInHierarchy && _battleStart)
        //        {
        //            if (_hookcalclated)
        //            {
        //                if (_calcPower && !_trackBall && !_startCheck)
        //                {

        //                    //value = val.ReadValue<Vector2>();
        //                    //if (value.y > 0)
        //                    //{
        //                    //    _startCheck = true;

        //                    //    _slidertime += Time.deltaTime;
        //                    //    _powerSlider.value = Mathf.Lerp(_powerSlider.minValue, _powerSlider.maxValue, _slidertime / 0.1f);
        //                    //}
        //                }
        //            }
        //        }

        //    }
        //};

        //_gameactions.ButtonActions.IGTpowerAction.canceled += x =>
        //{


        //    if (!_gamePaused && !_pauseMenupanel.activeInHierarchy)
        //    {
        //        if (_canhit && _ball.activeInHierarchy && _battleStart)
        //        {
        //            if (_hookcalclated)
        //            {
        //                if (_calcPower && !_trackBall)
        //                {
        //                 //   GetPowerValue();
        //                    Debug.Log("CANCLED");
        //                }
        //            }
        //        }

        //    }

        //};
        //#endregion

    }

    public void OnPowerOrDriftAction()
    {
        if (_hookcalclated)
        {
            OnPowerUpAction();
        }
        else
        {
            OnDriftAction();
        }
    }
    private void OnDriftAction()
    {
        if (_gamePaused || _pauseMenupanel.activeInHierarchy) return;
        if (_canhit && _ball.activeInHierarchy && _battleStart)
        {
            if (_hookcalclated == false)
            { // && !_trackBall){

                GetDriftValue();
                _hookcalclated = true;
            }
        }
    }

    private void OnPowerUpAction()
    {
        if (_gamePaused || _pauseMenupanel.activeInHierarchy) return;
        if (_canhit && _ball.activeInHierarchy && _battleStart)
        {
            if (_hookcalclated)
            {
                if (_calcPower && !_trackBall)
                {
                    if (!IGT)
                    {

                        GetPowerValue();
                    }

                }
            }
        }
    }

    public void PowerUpAction()
    {
        if (!_gamePaused && !_pauseMenupanel.activeInHierarchy)
        {
            if (_canhit && _ball.activeInHierarchy && _battleStart)
            {
                if (_hookcalclated)
                {
                    if (_calcPower && !_trackBall)
                    {
                        if (!IGT)
                        {
                            GetPowerValue();
                        }

                    }
                }
            }
        }
    }
    void Update()
    {

      
        /*  if(_myVoice.RecorderInUse.IsCurrentlyTransmitting){

             _isspeakingButt.SetActive(true);
             _notSpeakingButt.SetActive(false);
         }else{
               _isspeakingButt.SetActive(false);
             _notSpeakingButt.SetActive(true);
         } */
        if (_photonview.IsMine)
        {

            if (Application.isMobilePlatform)
            {
                OnMobileMove();
                OnHookAction(new Vector2(hookStick.Horizontal, 0));
            }
            if (_trackBall)
            {
                // _hookScroll.gameObject.SetActive(false);
                _powerSliderImg.gameObject.SetActive(false);
            }
            else
            {
                _powerSliderImg.gameObject.SetActive(true);
            }

            if (_gameend)
            {
                if (_MyPlayCanavas.activeInHierarchy)
                {
                    _MyPlayCanavas.SetActive(false);
                }
                if (!myleader.activeInHierarchy)
                {
                    myleader.SetActive(true);
                }
                if (GameModes._arcadeMode && !publishGameEnd)
                {

                    GameEventBus.Publish(GameEventType.arcademode);
                    publishGameEnd = true;
                }
                else if(!GameModes._arcadeMode)
                {
                    if (!_GoHomebutt.activeInHierarchy)
                    {
                        _GoHomebutt.SetActive(true);
                    }
                    EventSystem.current.SetSelectedGameObject(_GoHomebutt);
                }
             
                 

            }
           
            if (_canhit)
            {
               
                if (_readyLunch)
                {
                    if (!_fireball)
                    {
                        _myRocket?.GetComponent<GunfireController>().SetPlayer(this);
                        _myRocket?.GetComponent<GunfireController>().FireWeapon();
                       
                    }
                 
                
                   // Waitstate();
                    _readyLunch = false;
                   
                }
                if(!_hookcalclated){


                    UpdateHookSlider();
                    //if (!_trackBall)
                    //{
                    //    UpdateHookSlider();
                    //}
                    //else
                    //{
                    //    UpdateHookSlider();
                    //    var inp = Input.GetAxis("Mouse Y");
                    //    if(inp > 0)
                    //    {
                    //        GetDriftValue();
                    //    }
                    //}
                }
                else{
                       _hookScrollRect.gameObject.SetActive(false);
                }
                if(_hookcalclated  && !_powerval){
                  
                    if (!IGT && !_trackBall)
                    {
                        UpdateGui();
                    }
                  
                    else if(IGT || _trackBall)
                    {
                     //   if (_trackBall) { _filledImage.gameObject.SetActive(false); }
                      
                        if (_startCheck && IGT)
                        {
                            if (Input.GetMouseButtonUp(0))
                            {

                                Debug.Log("Touch Up");
                               GetPowerValue();
                            }
                        }
                        if (IGT)
                        {
                            if (Input.GetMouseButton(0))
                            {
                                Debug.Log("HOLDINGGG");
                                force += (Input.GetAxis("Mouse Y") * Time.deltaTime * 300);
                                _filledImage.fillAmount = force / 100;
                                _startCheck = true;
                            }
                        }else if (_trackBall)
                        {
                            
                            if(Input.GetAxis("Mouse Y") > 0)
                            {
                                force += (Input.GetAxis("Mouse Y") * Time.deltaTime);
                                _trackBallImage.fillAmount = force;
                                if (!_waitTrackBall)
                                {

                                    StartCoroutine(WaitTrackBall());
                                    _waitTrackBall = true;
                                }
                            }
                           

                         

                        }


                        }

                    }
                if(_hookcalclated && _powerval && !spinCalculated)
                {
                    OnUpdateSpinGui();

                }

            
                if (!_usingRock)
                {
                    if (!_gamePaused && !_hookcalclated)//when to move
                    {
                        inputdir = new Vector3(_movingL.x , 0, 0);
                        //Debug.Log("Moving");
                        //Debug.Log(_movingL);
                        transform.Translate(inputdir * Time.deltaTime);
                        Vector3 clampedPosition = transform.position;
                        clampedPosition.x = Mathf.Clamp(clampedPosition.x, _myxpos - 0.4f, _myxpos + 0.4f);
                        transform.position = clampedPosition;
                    }
                }
          
            }

        }
      
       
       
    }
    public bool IamTheOnlyOne()
    {

        var checkPlayers = _modePlayers.All(x => x.gameObject == null);
        return checkPlayers;
    }
    private void BowlState()
    {
       if(!_gameend){
        _playercontext.Transition(_BowlingState);
            Debug.Log("Bowl state");
       }
    }

    public void Waitstate()
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
                if (obj.name != "PinSetter")
                {
                    _mypins.Add(obj);
                    _resetpinsrot.Add(obj.transform.rotation);
                }
                else if(obj.name == "PinSetter")
                {
                    myPinsSetter = obj.gameObject;
                }
            }

            StartCoroutine(waitReady());
        }

    }
    IEnumerator WaitTrackBall()
    {
        
        yield return new WaitForSeconds(0.3f);
        GetPowerValue();
       
    }
    IEnumerator waitReady()
    {
        ResetPins(false);
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
        if (GameModes._rankedMode || GameModes._battleRoyale)
        {
            _modePlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
            if (_modePlayers.Contains(this.gameObject))
            {
                _modePlayers.Remove(this.gameObject);
            }
        }
        

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
      
        if(_powerSliderImg.fillAmount == 1){

            _ControllPower = true;
             _slidertime =0;
      
        }
        if(_powerSliderImg.fillAmount == 0 ){
           _slidertime =0;

         _ControllPower = false;
           
        }
         
        if(_ControllPower == false){
              _slidertime += Time.deltaTime;
            _powerSliderImg.fillAmount = Mathf.Lerp(0, 1, _slidertime / 0.3f);
        }else{
         
              _slidertime += Time.deltaTime;
            _powerSliderImg.fillAmount = Mathf.Lerp(1, 0, _slidertime / 0.3f);
        }
        
         
    }
    IEnumerator ActivePowerShot(){

        yield return new WaitForSeconds(0.5f);
         _calcPower = true;
    }
    private void GetPowerValue() {
        if (_powerval) return;
        if (_startCheck) { _startCheck = false; }

        if (!IGT &&!_trackBall)
        {
            _powerval = true;
            _power = Mathf.Lerp(minBowlPower,maxBowlPower,_powerSliderImg.fillAmount); // _powerSlider.value;

        }
        else if(IGT || _trackBall)
        {
            

            if (!_trackBall)
            {
                _waitTrackBall = false;
                //igt
                var powerval1 = _filledImage.fillAmount.ToString("F1");
                _power = GetActualPowerVal(float.Parse(powerval1));
            }
            else
            {
                //trackball
                var powerval2 = _trackBallImage.fillAmount.ToString("F1");
                presistencePower =GetActualPowerVal(float.Parse(powerval2));
                waitingPUSH = true;
                Debug.Log(_trackBallImage.fillAmount.ToString("F1"));
                Debug.Log(presistencePower);
            }

        }

        //check spin meter ?
        spinScroll.gameObject.SetActive(true);
      //    BowlState();
    }

    private void OnUpdateSpinGui()
    {
      //  spinScroll.value = Mathf.Lerp();
        if (_moverightspin == false)
        {

            //  _scrolltime = 0; 
            _spinTime += Time.deltaTime;
            spinScroll.value = Mathf.Lerp(0, 1f, _spinTime / spinDuration);

            if (spinScroll.value >= (0.95f))
            {
                _spinTime = 0;
                _moverightspin = true;
               
            }
        }
        else
        {
            _spinTime += Time.deltaTime;
            spinScroll.value = Mathf.Lerp(1f, 0, _spinTime / spinDuration);

            if (spinScroll.value <= (0.05))
            {
                _spinTime = 0;
                _moverightspin = false;
            }

        }
        
    }
    private void OnGetSpinValue()
    {
        if (_gamePaused || _pauseMenupanel.activeInHierarchy) return;
        if (!spinCalculated)
        {
            spinCalculated = true;
            spinvalue = Mathf.Lerp(spinMinValue,spinMaxValue, spinScroll.value);
            Debug.Log("Spin value = " + spinvalue);
            BowlState();
        }
    }
    private float GetActualPowerVal(float val)
    {
       // Debug.Log(val);
        switch (val)
        {
            case 0f:
                return minPower;
                
            case 0.1f:
                return minPower + 10;
               
            case 0.2f:
                return minPower + 20;
                
            case 0.3f:
                return minPower + 30;
                
            case 0.4f:
                return minPower + 40;
               
            case 0.5f:
                return minPower + 50;
               
            case 0.6f:
                return minPower + 55;
               
            case 0.7f:
                return minPower + 60;
               
            case 0.8f:
                return minPower + 65;
            
            case 0.9f:
                return minPower + 70;
                
            case 1:
                return minPower + 75;
                
        }
        return minPower;

    }
    
    private void UpdateHookSlider()
    {
         if(_moveright == false){
          
          //  _scrolltime = 0; 
            _scrolltime += Time.deltaTime;
            _hookScrollRect.anchoredPosition =/* Mathf.Lerp(_hookScroll.value, 1f, _scrolltime / 0.168f);*/  new Vector2(

                 Mathf.Lerp(minHookX, maxHookX, _scrolltime / hookDuration)
                , _hookScrollRect.anchoredPosition.y);
            
            if(_hookScrollRect.anchoredPosition.x >=  (maxHookX - 2f) ){
                _moveright = true;
                _scrolltime = 0;
            }
         }else{
       // _scrolltime = 0; 
            _scrolltime += Time.deltaTime;
            //  _hookScroll.value = Mathf.Lerp(_hookScroll.value, 0f, _scrolltime / 0.168f);//0.3
            _hookScrollRect.anchoredPosition =/* Mathf.Lerp(_hookScroll.value, 1f, _scrolltime / 0.168f);*/  new Vector2(

                 Mathf.Lerp(maxHookX, minHookX, _scrolltime / hookDuration)
                , _hookScrollRect.anchoredPosition.y);
            if (_hookScrollRect.anchoredPosition.x <= (minHookX +2)){
                _scrolltime = 0;
                _moveright = false;
            }

         }

    }
  
    private void GetDriftValue()
    {


        if(_hookScrollRect.anchoredPosition.x < 2 && _hookScrollRect.anchoredPosition.x > -2)//threshhold
        {
            _driftvalue = 0;
        }
        else
        {
            float driftval = _hookScrollRect.anchoredPosition.x / (Mathf.Abs(minHookX)); //(Mathf.Round((( (Mathf.Abs(_hookScrollRect.anchoredPosition.x)) / (Mathf.Abs(minHookX))) )/* * 10*/));//normalize to 1
            Debug.Log("Drift value = " + driftval);
            GetDrifRealVal(driftval);
        }
           _hookcalclated = true;
        _hookScrollRect.gameObject.SetActive(false);
        _powerSliderImg.gameObject.SetActive(true);
           StartCoroutine(ActivePowerShot());
    }
    private void GetDrifRealVal(float val)
    {
        _driftvalue = val > 0 ? Mathf.Lerp(0, _driftMaxval, Mathf.Abs(val)) : Mathf.Lerp(0, _driftMaxval * -1, Mathf.Abs(val));   // Mathf.Lerp(-_driftMaxval,_driftMaxval, val);
        Debug.Log("Drift value Final = " + _driftvalue);
        //switch (val)
        //{
        //    case 0:
        //        _driftvalue = (_driftMaxval * -1);
        //        break;
        //    case 1:
        //        _driftvalue = (_driftMaxval * -1);//ex : -100
        //        break;
        //    case 2:
        //        _driftvalue = (_driftMaxval * -1)+ 20;//ex -80
        //        break;
        //     case 3:
        //        _driftvalue = (_driftMaxval * -1) + 40;
        //        break;
        //    case 4:
        //        _driftvalue = (_driftMaxval * -1) + 60;
        //        break;
        //    case 5:
        //        _driftvalue =0;
        //        break;
        //    case 6:
        //        _driftvalue = _driftMaxval  - 60;
        //        break;
        //    case 7:
        //        _driftvalue = _driftMaxval - 40;
        //        break;
        //    case 8:
        //        _driftvalue = _driftMaxval - 20;
        //        break;
        //    case 9:
        //        _driftvalue = _driftMaxval ;
        //        break;
        //    case 10:
        //        _driftvalue = _driftMaxval;
        //        break;


        //}
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
    private void ResetPins(bool pinResetter)
    {
    
        if (_photonview.IsMine)
        {

            photonView.RPC("ResetPinsRunCounter", RpcTarget.All);

            if (pinResetter)
            {
                PinSetterReset();//PinSetter
            }
        }
       


    }
    [PunRPC]
    private void ResetPinsRunCounter()//Reset After All Down Or New Round
    {
        for (int y = 0; y < _resetpins.Count; y++)
        {
            if (_mypins[y].gameObject.activeInHierarchy == false)
            {
               // _mypins[y].gameObject.SetActive(true);
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
    public void PinSetterReset()
    {
       
        StartCoroutine(PinSetterWaiter());

    }
    IEnumerator PinSetterWaiter()
    {
        myPinsSetter.GetComponent<Animator>().Play("Down");//Do Cycle Twice
        yield return new WaitForSeconds(0.6f); // down time
        foreach (Transform pin in _mypins)
            pin.transform.parent = myPinsSetter.transform.Find("Collector");
        yield return new WaitForSeconds(0.6f); //up time
        myPinsSetter.transform.Find("Collector").gameObject.SetActive(true);
        foreach (Transform pin in _mypins)
        {
            pin.gameObject.SetActive(true);
        }
        myPinsSetter.GetComponent<Animator>().Play("Down");
        yield return new WaitForSeconds(0.6f); // down time
       foreach (Transform pin in _mypins)// Reback Pins Parent
            pin.transform.parent = _mypinsobj.transform;
        yield return new WaitForSeconds(0.6f); //up time
        myPinsSetter.transform.Find("Collector").gameObject.SetActive(false);
    }
   
    public void Bowl(int pinFall)
    {
        if (GameModes._battleRoyale)
        {
            CheckRoyalCondition(pinFall);
        }
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
    
    private void CheckRoyalCondition(int pins)
    {
        if(pins != 10)
        {
            _trackScore += pins;
            _checkCond++;
        }
        else
        {

            _trackScore = 0;
            _checkCond = 0;
        }
        if(_checkCond >= 2)
        {
            if(_trackScore != 10 && !_rankedPanel.activeInHierarchy)
            {
                _gameend = true;
                ShowRankedResult("lose2");
            }
            else
            {
                _trackScore = 0;
                _checkCond = 0;
            }
        }

    }
  
    public void PerformAction(ActionMasterOld.Action action)
    {
         if (action == ActionMasterOld.Action.EndTurn)
        {

            ResetPins(true);
        }
        else if (action == ActionMasterOld.Action.Reset)
        {

            ResetPins(true);
        }
        else if (action == ActionMasterOld.Action.EndGame)
        {

            GameFinished();
            throw new UnityException("Don't know how to handle end game yet");
        }
    }
    private void GameFinished()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            StartCoroutine(PostScore());
        }
        _gameend = true;
    }
  
    public void CheckWinner()
    {
        if (_photonview.IsMine)
        {
            if (_modePlayers.Count > 0 && !_rankedPanel.activeInHierarchy)
            {
               
              
                if (_modePlayers[0] != null)
                {
                    var findscore = _modePlayers[0].GetComponent<PlayerController>();
                    if (findscore._gameend)
                    {
                        _waitOtherPlayer.SetActive(false);
                        if (_scoreplayer.totalscre > findscore._scoreplayer.totalscre)
                        {
                           
                            ShowRankedResult("win");
                        }
                        else if (_scoreplayer.totalscre < findscore._scoreplayer.totalscre)
                        {
                           
                            ShowRankedResult("lose");
                        }
                        else if (_scoreplayer.totalscre == findscore._scoreplayer.totalscre)
                        {
                           
                            ShowRankedResult("draw");
                        }
                       
                        _gameRankedFinished = true;
                    }
                    else
                    {
                      
                        _waitOtherPlayer.SetActive(true);
                    }
                }
            }
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (GameModes._rankedMode)
        {
            if (stream.IsWriting)
            {
               
              
               
                stream.SendNext(_gameend);
                stream.SendNext(_scoreplayer.totalscre);
            }
            else
            {
              
                this._gameend = (bool)stream.ReceiveNext();
                this._scoreplayer.totalscre = (int)stream.ReceiveNext();
            }
        }

    }
    #region Inputs
    public void OnMobileMove()
    {
        if (!Application.isMobilePlatform) return;
        if (joyStick == null) return;
        if (_gamePaused || _pauseMenupanel.activeInHierarchy) return;
            _movingL = new Vector3(joyStick.Horizontal, 0, 0);
    }
    #endregion
    #region UI
    public void Pause()
    {
        _pauseMenupanel.SetActive(true);
        _gamePaused = true;
        EventSystem.current.SetSelectedGameObject(_pauseMenuFirstbutt);
    }
    #endregion

    #region Settings
    public void Resume(){

        _pauseMenupanel.SetActive(false);
       StartCoroutine(WaitPause());
    }
    IEnumerator WaitPause(){
        yield return new WaitForSeconds(1);
        _gamePaused = false;
    }
    public void QuitGame(){

        LeavePanetly();
        StartCoroutine(wait());

      
    }
    private IEnumerator wait()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.AutomaticallySyncScene = false;
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }
    public void SoundOn(){
    
        AudioListener.volume = 1;
          EventSystem.current.SetSelectedGameObject(_soundOnOF[1]);

    }
     public void Soundoff(){
    
        AudioListener.volume = 0;
        EventSystem.current.SetSelectedGameObject(_soundOnOF[0]);
    }
    public void TrackBallOn()
    {
        _trackBall = true;
        _progressPowerUi.SetActive(true);
        _power = 0;
        EventSystem.current.SetSelectedGameObject(_trackBallOnOf[0]);
    }
    public void TrackBallOff()
    {
        _trackBall = false;
        _progressPowerUi.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_trackBallOnOf[1]);
    }
    #endregion
    IEnumerator PostScore() {
        // form data settings
        // field order doesn't matter, but field names must be correct

        WWWForm form = new WWWForm();
        form.AddField("request",    "save");                // must use 'save' - lowercase
        form.AddField("game",       Globals.game_id);       // game id
        form.AddField("user",      PhotonNetwork.LocalPlayer.NickName);           // user name
      //  form.AddField("gamescore",     _scoreplayer.totalscre);          // game score
        form.AddField("score", PlayerPrefs.GetInt("rankedpoints"));
     //   form.AddField("mac",        Globals.GetMacAddr());  // device MAC address

        using (UnityWebRequest request = UnityWebRequest.Post(Globals.serverURL, form)) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                
            }
        }

      
    }
    public void ShowRankedResult(string state) 
    {
        if (_MyPlayCanavas.activeInHierarchy)
        {
            _MyPlayCanavas.SetActive(false);
        }
        if (_saveCbutt.activeInHierarchy)
        {
            _saveCbutt.SetActive(false);
        }

           myleader.SetActive(true);
          _GoHomebutt.SetActive(true);
          _rankedPanel.SetActive(true);
          EventSystem.current.SetSelectedGameObject(_GoHomebutt);
        
        int latpoints = PlayerPrefs.GetInt("rankedpoints");
        switch (state)
        {
            case "win":

                _rankedstatetxt.text = "You Win !";
                _rankedpointtxt.text = "+2 ";
             
                PlayerPrefs.SetInt("rankedpoints", latpoints + 2);
                break;
            case "lose":
                _rankedstatetxt.text = "You Lose";
                _rankedpointtxt.text = "-1 ";
               
               PlayerPrefs.SetInt("rankedpoints", latpoints - 1);
                break;
            case "draw":
                _rankedstatetxt.text = "Draw !!";
                _rankedpointtxt.text = "+1 ";
                PlayerPrefs.SetInt("rankedpoints", latpoints + 1);
                break;
            case "win2":
                _rankedstatetxt.text = "You Win !";
                _rankedpointtxt.text = "+10 ";
                PlayerPrefs.SetInt("rankedpoints", latpoints + 10);
                break;
            case  "lose2":
                _rankedstatetxt.text = "You Lose";
                _rankedpointtxt.text = "0";
                StartCoroutine(waitExit());

                break;
            case "timeout":
                _rankedstatetxt.text = "Time Out";
                _rankedpointtxt.text = "0 ";
                StartCoroutine(waitExit());
                break;
            case "arcade":
                
                _rankedstatetxt.text = "CONGRATULATIONS YOU WIN!";
                _rankedpointtxt.text = "+50 ";
                PlayerPrefs.SetInt("rankedpoints", latpoints + 50);
                break;
        }
        
        PlayerPrefs.Save();

         #if !UNITY_WEBGL
        if (!DataBaseManager.Instance.IsLocallSaving)
        {
            StartCoroutine(DataBaseManager.AddIndividualDataToUser(DataBaseManager.UserID, PhotonNetwork.LocalPlayer.NickName, "playerData", "rankedPoints", latpoints));

        }
#endif
        StartCoroutine(PostScore());
    }
    IEnumerator waitExit()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(wait());
    }
    private void OnApplicationQuit()
    {
        LeavePanetly();
    }
    private void LeavePanetly()
    {
        if (GameModes._rankedMode)
        {
           
            if (!_gameend )
            {
                if (_photonview.IsMine)
                {
                    int latpoints = PlayerPrefs.GetInt("rankedpoints");
                    PlayerPrefs.SetInt("rankedpoints", latpoints - 5);
                    StartCoroutine(PostScore());
                    PlayerPrefs.Save();
                }
                  


            }
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

      //  PlayerController _controll =;
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

