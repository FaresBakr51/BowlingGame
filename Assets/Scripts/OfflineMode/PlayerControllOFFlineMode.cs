using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BigRookGames.Weapons;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerControllOFFlineMode : MonoBehaviour
{
   public GameObject _ball;
   public Transform _playerhand;
   public Vector3 _BallConstantPos;
   public float _power;
   public GameObject _camera;
   private Animator _playerAnim;
   public Slider _powerSlider;
   public Scrollbar _hookScroll;
   public List<Transform> _mypins = new List<Transform>();
   private  List<Vector3> _resetpins = new List<Vector3>();
    [SerializeField] public List<Transform> _leftpins = new List<Transform>();
    private List<Quaternion> _resetpinsrot = new List<Quaternion>();
   public float _speed;
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
   
   private GameObject _frametextobj;
   private GameObject _framescoretextobj;
   
   public GameObject myleader;
   public GameObject _mypinsobj;
 

    public GameObject _GoHomebutt;
   public GameControls _gameactions;
   public bool _powerval;
   private bool _moveright;
   private Vector3 _movingL;
   
   public bool _calcScore;
   public bool _calcPower;
   private bool _ControllPower;
   private GetNickname _getMyname;
   private OfflinePlayerMode _offlinemode;
   public int _mycontroll;
   private bool _goforword;
   private bool _followBall;
   private Vector3 _mypos;
   private Vector3 _Cambos;
 
   public AudioSource _gameAudio;
   public AudioClip[] _gameClips;
   public AudioClip[] _FramesClips;

    public GameObject _strikeTxt;
    public GameObject _spareTxt;
    public GameObject _gutterTxt;
    [Header("SpecialWeponProp")]
    [SerializeField] public GameObject _myRocket;
    [SerializeField] public GameObject _RocketOff;
    [SerializeField] public GameObject _RocketOn;
    public float fireballspeed;
    public AudioClip _fireballSoundClip;
    public bool _usedRocket;
    public bool _readyLunch;
    private bool _usingRock;
    public bool _fireball;




    [SerializeField] private List<GameObject> _offlinePlayers;
    private bool _gameFinished;

    [SerializeField] private GameObject _pauseMenupanel;

    [Header("BallControll")]
    public bool _driftBall;
    [SerializeField] private float _controllBallPower;
    
    void Awake()
    {
           _mypos = this.transform.position;
          _offlinemode = FindObjectOfType<OfflinePlayerMode>();
         _canhit = true;
        _myxpos = transform.position.x;
        _scoreplayer = GetComponent<ScorePlayer>();
        _powerSlider.gameObject.SetActive(false);
        _hookScroll.gameObject.SetActive(false);
         _gameactions = new GameControls();
        _mypinsobj.transform.parent = null;
        _ball.GetComponent<TrailRenderer>().enabled = false;
        if (SceneManager.GetActiveScene().name == "Map3")
        {
            _mypinsobj.transform.position = new Vector3(_mypinsobj.transform.position.x, _mypinsobj.transform.position.y, _mypinsobj.transform.position.z - 0.6f);
        }
        else
        {
            _mypinsobj.transform.position = new Vector3(_mypinsobj.transform.position.x, _mypinsobj.transform.position.y, _mypinsobj.transform.position.z + 0.3f);
        }
    }
      public  void OnEnable()
    {
      
     
            _gameactions.Enable();
    }
     void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        CheckControlles();  
         _GoHomebutt =  myleader.GetComponentInChildren<Button>().gameObject;
        _GoHomebutt.SetActive(false);
        _hookScroll.gameObject.SetActive(true);
        _playerAnim = GetComponent<Animator>();
         GetReady();
         _Cambos = _camera.transform.position;
    

    }

    public  void OnDisable()
    {
       
          _gameactions.Disable();
    }
    private void CheckControlles(){




        _pauseMenupanel = _offlinemode._pauseMenupanel;
        if (_mycontroll == 1){
           
               _hookScroll.GetComponent<RectTransform>().anchoredPosition = new Vector2( _hookScroll.GetComponent<RectTransform>().anchoredPosition.x-720, _hookScroll.GetComponent<RectTransform>().anchoredPosition.y);
                _strikeTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(_strikeTxt.GetComponent<RectTransform>().anchoredPosition.x-350,_strikeTxt.GetComponent<RectTransform>().anchoredPosition.y);
                 _spareTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(_spareTxt.GetComponent<RectTransform>().anchoredPosition.x-350,_spareTxt.GetComponent<RectTransform>().anchoredPosition.y);
            _gutterTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(_gutterTxt.GetComponent<RectTransform>().anchoredPosition.x - 350, _gutterTxt.GetComponent<RectTransform>().anchoredPosition.y);
           
            FirstControll();
            }else if(_mycontroll ==0){
             _powerSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2( _powerSlider.GetComponent<RectTransform>().anchoredPosition.x+720, _powerSlider.GetComponent<RectTransform>().anchoredPosition.y);
              _strikeTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(_strikeTxt.GetComponent<RectTransform>().anchoredPosition.x+350,_strikeTxt.GetComponent<RectTransform>().anchoredPosition.y);
             _spareTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(_spareTxt.GetComponent<RectTransform>().anchoredPosition.x+350,_spareTxt.GetComponent<RectTransform>().anchoredPosition.y);
            _gutterTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(_gutterTxt.GetComponent<RectTransform>().anchoredPosition.x + 350, _gutterTxt.GetComponent<RectTransform>().anchoredPosition.y);
            _RocketOff.GetComponent<RectTransform>().anchoredPosition = new Vector2(_RocketOff.GetComponent<RectTransform>().anchoredPosition.x + 750, _RocketOff.GetComponent<RectTransform>().anchoredPosition.y);
            _RocketOn.GetComponent<RectTransform>().anchoredPosition = new Vector2(_RocketOn.GetComponent<RectTransform>().anchoredPosition.x + 750, _RocketOn.GetComponent<RectTransform>().anchoredPosition.y);
            SecondControll();
            }
            
        

    }
    public void UpdateSound(AudioClip clip){

     
        _gameAudio.PlayOneShot(clip);
        
    }
      private void FirstControll(){

              _gameactions.ButtonActions.moving.performed += cntxt => {

                  if (!_pauseMenupanel.activeInHierarchy)
                  {
                      _movingL = cntxt.ReadValue<Vector2>();
                  }
                  if (_driftBall)
                  {

                      if (_movingL.x > 0)
                      {

                          var rig = _ball.GetComponent<Rigidbody>();
                     
                          rig.AddForce(new Vector3(-_controllBallPower, 0, 0), ForceMode.Impulse);
                          _driftBall = false;

                      }
                      else if (_movingL.x < 0)
                      {

                          var rig = _ball.GetComponent<Rigidbody>();
                       
                          rig.AddForce(new Vector3(_controllBallPower, 0, 0), ForceMode.Impulse);
                          _driftBall = false;

                      }
                  }


              };
        _gameactions.ButtonActions.moving.canceled += cntxt => _movingL =Vector2.zero;

            _gameactions.ButtonActions.powerupaction.performed += x => {
                if (!_offlinemode._gamePaused && !_pauseMenupanel.activeInHierarchy)
                {
                    if (_canhit == true && _ball.activeInHierarchy)
                    {
                        if (_hookcalclated == true)
                        {
                            if (_calcPower == true)
                            {
                                GetPowerValue();
                            }
                        }
                    }
                }
           };
          _gameactions.ButtonActions.driftbar.performed += y => {
              if (!_offlinemode._gamePaused && !_pauseMenupanel.activeInHierarchy)
              {
                  if (_canhit == true && _ball.activeInHierarchy)
                  {
                      if (_hookcalclated == false)
                      {
                          GetDriftValue();
                          _hookcalclated = true;
                      }
                  }
              }
          };

        _gameactions.ButtonActions.Rocket2.performed += r =>
        {
            if (_canhit)
            {
                if (!_usedRocket)
                {
                    if (!_offlinemode._gamePaused)
                    {
                        _usingRock = true;
                        UpdateAnimator("shot", 2);
                        _myRocket?.SetActive(true);
                        transform.rotation = Quaternion.Euler(transform.rotation.x, 200, transform.rotation.z);
                        _ball?.SetActive(false);
                        this.transform.position = _mypos;
                        StartCoroutine(readyLunch());
                        _usedRocket = true;
                    }
                }
            }
        };

        _gameactions.ButtonActions.pause.performed += x => {
            if (!_gameend&& !_usingRock)
            {
                _pauseMenupanel.SetActive(true);
                _offlinemode._gamePaused = true;
                EventSystem.current.SetSelectedGameObject(_offlinemode.pausefirstbutt);
            }
        };
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
            _myRocket.GetComponent<ProjectileMover>().speed = fireballspeed;
            _readyLunch = true;
        }
        else
        {
            yield return new WaitForSeconds(2);
            _readyLunch = true;
        }
        yield return new WaitForSeconds(1.5f);

        foreach (Transform pin in _mypins)
        {
            pin.transform.rotation = Quaternion.Euler(pin.rotation.x, pin.rotation.y, Random.Range(90, 180));

        }

    }
    private void SecondControll(){
            _gameactions.ButtonActions.moving2.performed += cntxt => {

                if (!_pauseMenupanel.activeInHierarchy)
                {
                    _movingL = cntxt.ReadValue<Vector2>();
                }
                if (_driftBall)
                {

                    if (_movingL.x > 0)
                    {

                        var rig = _ball.GetComponent<Rigidbody>();
                
                        rig.AddForce(new Vector3(-_controllBallPower, 0, 0), ForceMode.Impulse);
                        _driftBall = false;

                    }
                    else if (_movingL.x < 0)
                    {

                        var rig = _ball.GetComponent<Rigidbody>();
                    
                        rig.AddForce(new Vector3(_controllBallPower, 0, 0), ForceMode.Impulse);
                        _driftBall = false;

                    }
                }


            };
        _gameactions.ButtonActions.moving2.canceled += cntxt => _movingL =Vector2.zero;

            _gameactions.ButtonActions.powerupaction2.performed += x => {
                if (!_offlinemode._gamePaused && !_pauseMenupanel.activeInHierarchy)
                {
                    if (_canhit == true && _ball.activeInHierarchy)
                    {
                        if (_hookcalclated == true)
                        {
                            if (_calcPower == true)
                            {
                                GetPowerValue();
                            }
                        }
                    }
                }
           };
          _gameactions.ButtonActions.driftbar2.performed += y => {
              if (!_offlinemode._gamePaused && !_pauseMenupanel.activeInHierarchy)
              {
                  if (_canhit == true && _ball.activeInHierarchy)
                  {
                      if (_hookcalclated == false)
                      {
                          GetDriftValue();
                          _hookcalclated = true;
                      }
                  }
              }
              
          };
        _gameactions.ButtonActions.Rocket.performed += r =>
        {
            if (_canhit)
            {
                if (!_usedRocket)
                {
                    if (!_offlinemode._gamePaused)
                    {
                        _usingRock = true;
                        UpdateAnimator("shot", 2);
                        _myRocket?.SetActive(true);
                        transform.rotation = Quaternion.Euler(transform.rotation.x, 200, transform.rotation.z);
                        _ball?.SetActive(false);
                        this.transform.position = _mypos;
                        StartCoroutine(readyLunch());
                        _usedRocket = true;
                    }
                }
            }
        };
    }

private void GetReady()
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
        _offlinePlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
        if (_offlinePlayers.Contains(this.gameObject))
        {
            _offlinePlayers.Remove(this.gameObject);
        }
        foreach (Transform framtxt in _frametextobj.GetComponentInChildren<Transform>())
        {
            _scoreplayer.scores_text.Add(framtxt.gameObject.GetComponent<Text>());

        }
        foreach (Transform framtxtscore in _framescoretextobj.GetComponentInChildren<Transform>())
        {
            _scoreplayer.round_scores_text.Add(framtxtscore.gameObject.GetComponent<Text>());

        }
    }
    
    void Update()
    {
    
        if(_goforword)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            this.transform.Translate(Vector3.forward * Time.deltaTime *_speed);
            
        }
         if (_followBall)
        {
           
            if (_camera.transform.position.z >= _mypinsobj.transform.position.z +10)
            {
                _camera.transform.position = _ball.transform.position + new Vector3(0, 1, 1);

                //       _camera.transform.position = _ball.transform.position + new Vector3(-2.1f, 1.5f, -3);

                //                _camera.transform.localEulerAngles = new Vector3(30, -135,_camera.transform.localEulerAngles.z);
            }
            else
            {
                _followBall = false;
            }
           
        }
        if (_gameend)
        {
            if (_offlinePlayers.Count > 0)
            {
                _MyPlayCanavas.SetActive(false);
                var findscore = _offlinePlayers[0].GetComponent<PlayerControllOFFlineMode>();
                if (findscore._gameend && !_gameFinished)
                {

                    _GoHomebutt.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(_GoHomebutt);
                    _gameFinished = true;
                }
            }
        }
        if (_canhit)
        {
            if (_readyLunch)
            {


                if (!_fireball)
                {
                    _myRocket?.GetComponent<GunfireController>().FireWeapon();
                }


                 WaitState();
                _readyLunch = false;

            }
            if (_hookcalclated == false)
            {

                UpdateHookSlider();
            }
            else
            {
                _hookScroll.gameObject.SetActive(false);
            }
            if (_hookcalclated == true && _powerval == false)
            {

                UpdateGui();
            }
            if (!_usingRock)
            {
                if (!_offlinemode._gamePaused) {
                    inputdir = new Vector3(_movingL.x, 0, 0);
                transform.Translate(inputdir * Time.deltaTime);
                Vector3 clampedPosition = transform.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, _myxpos - 0.4f, _myxpos + 0.4f);
                transform.position = clampedPosition;
                  }
            }
        }

        
       
       
    }
     public void UpdateAnimator(string val, int value)
    {
        _playerAnim.SetInteger(val, value);
    }
    private void UpdateGui()
    {
        if(_powerSlider.value == _powerSlider.maxValue){

            _ControllPower = true;
             _slidertime =0;
      
        }
        if(_powerSlider.value == _powerSlider.minValue ){
           _slidertime =0;

         _ControllPower = false;
           
        }
         
        if(!_ControllPower){

             
              _slidertime += Time.deltaTime;
              _powerSlider.value = Mathf.Lerp(_powerSlider.minValue, _powerSlider.maxValue, _slidertime / 0.3f);
        }else{
         
              _slidertime += Time.deltaTime;
             _powerSlider.value = Mathf.Lerp(_powerSlider.maxValue, _powerSlider.minValue, _slidertime / 0.3f);
        }
        
         
    }
    IEnumerator ActivePowerShot(){

        yield return new WaitForSeconds(0.5f);
         _calcPower = true;
    }
    private void GetPowerValue(){
          _powerval = true;
           _power = _powerSlider.value;
       ShotBall();
    }
    private void UpdateHookSlider()
    {
         if(_moveright == false){
           
             _scrolltime = 0; 
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 1f, _scrolltime / 0.168f);
            
            if(_hookScroll.value >= 0.9){
                _moveright = true;
            }
         }else{
        _scrolltime = 0; 
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 0f, _scrolltime / 0.168f);

            if(_hookScroll.value <= 0.1f){

                _moveright = false;
            }

         }

    }
    private void GetDriftValue()
    {
        //if (_hookScroll.value < 0.45f)//1,2,3,4///30///120
        //{
        //    float driftval = ((_hookScroll.value * 10) * -_driftmaxvaluleft);
        //    _driftvalue = driftval;
        //}
        //if (_hookScroll.value > 0.45f)//4,5,6,7,8/////15///120
        //{
        //    float driftval = ((_hookScroll.value * 10) * _driftmaxvaluright);
        //    _driftvalue = driftval;
        //}
        //if (_hookScroll.value == 0.45f)
        //{
        //    _driftvalue = 0;
        //}
        if (_hookScroll.value == 0.45f)
        {
            _driftvalue = 0;
        }
        else
        {
            float driftval = (Mathf.Round(_hookScroll.value * 10));
            GetDrifRealVal(driftval);
        }

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
    public void ShotBall()
    {
        _canhit = false;
        UpdateAnimator("shot", 1);
      StartCoroutine(WaitGrounded());
        
     
        
    }
     IEnumerator WaitGrounded()
    {
        _goforword = true;
        yield return new WaitForSeconds(1.06f);
        _ball.GetComponent<Rigidbody>().isKinematic = false;
        _ball.transform.parent = null;
        _goforword = false;
       UpdateAnimator("shot", 0);
        _ball.GetComponent<BallSound>().enabled = true;
        var rig = _ball.GetComponent<Rigidbody>();
        rig.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rig.AddForce(new Vector3(0, 0, -_power), ForceMode.Impulse);
        rig.AddForce(new Vector3(-_driftvalue, 0, 0), ForceMode.Force);
        _ball.GetComponent<TrailRenderer>().enabled = true;
        WaitState();
      
    }
    private void WaitState(){
        
        _MyPlayCanavas.SetActive(false);
        if (_ball.activeInHierarchy)
        {
            _followBall = true;

            _ball.GetComponent<BallSound>().UpdateSound(_movingclip);
        }
        if (!_usingRock)
        {

            UpdateAnimator("shot", 7);
        }
       _driftBall = true;

        StartCoroutine(WaitHit());

        
    }
       IEnumerator WaitHit()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(ChechPins());
        

    }
      private IEnumerator ChechPins()
    {

        yield return new WaitForSeconds(4f);
        for (int i = 0; i < _mypins.Count; i++)
        {

            if (_mypins[i].transform.eulerAngles.y > 10 || _mypins[i].transform.eulerAngles.y > 10 || _mypins[i].transform.eulerAngles.z > 10)//_mypins[i].transform.up.y < 0.9f)//|| playercontroller._resetpins[i].x != playercontroller._mypins[i].transform.localPosition.x)// Mathf.Abs(playercontroller._mypins[i].transform.rotation.eulerAngles.z) > 5f
            {
                if (_mypins[i].gameObject.activeInHierarchy == true)
                {
                    _roundscore += 1;
                    _mypins[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if (_mypins[i].gameObject.activeInHierarchy == true)
                {
                    _mypins[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    _leftpins.Add(_mypins[i]);
                    _mypins[i].gameObject.SetActive(false);
                }
            }
        }

        StartCoroutine(Waittopublish());

    }
      private void CheckOtherHit()
    {

      
        AddToLeaderBoard();
        IdleState();
    }
    private void IdleState(){

       
     

         StartCoroutine(WaitToReset());

    }
        IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(1.5f);

        myleader.SetActive(true);
      
        if (_leftpins.Count > 0)
        {
            foreach (Transform leftpin in _leftpins)
            {
                leftpin.transform.rotation = Quaternion.identity;
                if (leftpin.gameObject.GetComponent<Rigidbody>().isKinematic)
                {
                    leftpin.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                leftpin.gameObject.SetActive(true);
                leftpin.transform.up = new Vector3(leftpin.transform.up.x, 1, leftpin.transform.up.z);
            }
        }
        yield return new WaitForSeconds(3f);
        myleader.SetActive(false);
        _MyPlayCanavas.SetActive(true);
        ResetCamAndpins();
       
    }
     private void ResetCamAndpins()
    {
        
        _powerval = false;
        _hookcalclated = false;
        _powerSlider.gameObject.SetActive(false);
        _hookScroll.gameObject.SetActive(true);
        _calcPower = false;
        _ball.GetComponent<Rigidbody>().isKinematic = true;
        _ball.GetComponent<BallSound>().enabled = false;
        _ball.transform.parent = _playerhand;
        _slidertime = 0;
        _scrolltime = 0;
        _powerSlider.value = 0;
        _hookScroll.value = 0.5f;
        _camera.transform.position = _Cambos;
        _ball.transform.localPosition = _BallConstantPos;
     
         _ball.GetComponent<TrailRenderer>().enabled = false;
        if (_gameend)
        {
            _canhit = false;
            myleader.SetActive(true);
            _GoHomebutt.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_GoHomebutt);
            if (_scoreplayer.totalscre >= 300)
            {
                UpdateSound(_gameClips[7]);
            }
        }
        else
        {
            UpdateAnimator("shot", 0);
            _canhit = true;
        }
       
        if (_usingRock)
        {
            
            _RocketOn.SetActive(false);
            _RocketOff.SetActive(true);
            _usingRock = false;
            _myRocket.SetActive(false);
            _ball.SetActive(true);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            UpdateAnimator("shot", 0);
             _camera.transform.position = _Cambos;
        }
        _roundscore = 0;
        _ball.GetComponent<BallSound>()._hit = false;
        this.transform.position = _mypos;
        _camera.transform.position = _Cambos;
   
      
        foreach (Transform pins in _mypins)
        {
            if (pins.name != "PinSetter")
            {
                pins.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            //    pins.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }
    }

   
    private void AddToLeaderBoard()
    {
        Bowl(_roundscore);
    }
      IEnumerator Waittopublish()
    {
        yield return new WaitForSeconds(1f);
       CheckOtherHit();
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
 private void ResetPins()
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
