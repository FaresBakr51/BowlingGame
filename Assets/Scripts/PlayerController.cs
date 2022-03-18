using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Voice.PUN;
using UnityEngine.Networking;
using BigRookGames.Weapons;
using System.Linq;
public class PlayerController : MonoBehaviourPunCallbacks,IPunObservable
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
    private List<Quaternion> _resetpinsrot = new List<Quaternion>();
    public float _speed;
    private PlayerStateContext _playercontext;
    private PlayerState _waitingState, _BowlingState, _idleState;
    public float _slidertime;
    public float _scrolltime;
    private Vector3 inputdir;
    public GameObject _MyPlayCanavas;
    private float _myxpos;
    public bool _hookcalclated;
    public float _driftvalue;
    [SerializeField] private float _driftmaxvalu;
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
    public GameObject _mytotal;
    public GameObject _GoHomebutt;
    public GameActions _gameactions;
    public bool _powerval;
    private bool _moveright;
    private Vector3 _movingL;
   
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
   
    [SerializeField] private PhotonVoiceView _myVoice;
    public GameObject _isspeakingButt;
    public GameObject _notSpeakingButt;
    public GameObject _myManager;

  
    [SerializeField] private GameObject _rankedPanel;
    [SerializeField] private Text _rankedpointtxt;
    [SerializeField] private Text _rankedstatetxt;
 
    [SerializeField] public GameObject _myRocket;
    [SerializeField] public GameObject _RocketOff;
    [SerializeField] public GameObject _RocketOn;
    public bool _usedRocket;
    public bool _readyLunch;
    [SerializeField] private List<GameObject> _rankedPlayers;
    private bool _gameRankedFinished;
    public bool _usingRock;
    private Vector3 _mypos;
    private void Awake()
    {

        //  _myVoice = GetComponent<PhotonVoiceView>();
        _mypos = this.transform.position;
        _powerSlider.gameObject.SetActive(false);
        _hookScroll.gameObject.SetActive(false);
         _gameactions = new GameActions();
        _photonview = GetComponent<PhotonView>();
        _mypinsobj.transform.parent = null;
        _golballeaderboradcanavas = GameObject.FindWithTag("leaderboard");
          _scoreplayer = GetComponent<ScorePlayer>();
        if (!_photonview.IsMine)
        {
          _camera.GetComponent<Camera>().enabled = false;
          _camera.GetComponent<AudioListener>().enabled = false;
         GetComponent<PlayerController>().enabled = false;
            Destroy(_MyPlayCanavas);

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
         _gameactions.ButtonActions.moving.performed += cntxt =>{
             
             if(!_gamePaused && !_pauseMenupanel.activeInHierarchy){
              _movingL = cntxt.ReadValue<Vector2>();
             }
              
              };
           _gameactions.ButtonActions.moving.canceled += cntxt => _movingL =Vector2.zero;
        _gameactions.ButtonActions.pause.performed += x => {
            if (!_gameend) {
                _pauseMenupanel.SetActive(true);
                _gamePaused = true;
                EventSystem.current.SetSelectedGameObject(_pauseMenuFirstbutt);
            }
        };
        _gameactions.ButtonActions.Rocket.performed += r =>
        {
            if (_canhit)
            {
                if (!_usedRocket)
                {
                    Debug.Log("pRESSED TRIANGLE");
                    _usingRock = true;
                    transform.position = _mypos;
                    UpdateAnimator("shot", 2);
                    _myRocket?.SetActive(true);
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 200, transform.rotation.z);
                    _ball?.SetActive(false);
                    StartCoroutine(readyLunch());
                    _usedRocket = true;

                }
            }
        };
           
            _gameactions.Enable();
    }
    IEnumerator readyLunch()
    {
        yield return new WaitForSeconds(2);
        _readyLunch = true;
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
           if(!PhotonNetwork.OfflineMode  || (PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)){
            myleader = PhotonNetwork.Instantiate("Panel", _leaderboardprefab.transform.position, _leaderboardprefab.transform.rotation);
           }
          _GoHomebutt =  myleader.GetComponentInChildren<Button>().gameObject;
            var rankedpanelobj = myleader.GetComponentsInChildren<Transform>();
            var rankedpanel = rankedpanelobj.FirstOrDefault(x => x.name == "RankedPanel");
            _rankedPanel = rankedpanel.gameObject;
            var rankedpanobj = rankedpanel.GetComponentsInChildren<Transform>();
            _rankedpointtxt = rankedpanelobj.FirstOrDefault(x => x.name == "rankedpoints").GetComponent<Text>();
            _rankedstatetxt = rankedpanelobj.FirstOrDefault(x => x.name == "rankedstate").GetComponent<Text>();
            _rankedPanel.SetActive(false);
            myleader.GetComponentInChildren<Button>().gameObject.SetActive(false);
            myleader.transform.parent = _golballeaderboradcanavas.transform;
            myleader.gameObject.SetActive(false);
            myleader.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100f);
            myleader.transform.GetComponent<RectTransform>().localScale = new Vector3(2, 2f, 2);

           
        }
     


        _hookScroll.gameObject.SetActive(true);
        _playercontext = new PlayerStateContext(this);
        _BowlingState = gameObject.AddComponent<PlayerBowlingState>();
        _waitingState = gameObject.AddComponent<PlayerWaitingState>();
        _idleState = gameObject.AddComponent<PlayerIdleState>();
        _playerAnim = GetComponent<Animator>();
         GetReady();
    }
    private void CheckControlles(){

           if(!PhotonNetwork.OfflineMode || (PhotonNetwork.OfflineMode &&PhotonNetwork.InRoom )){

             
             _gameactions.ButtonActions.powerupaction.performed += x => {
                  if(!_gamePaused && !_pauseMenupanel.activeInHierarchy){
                 if(_canhit == true && _ball.activeInHierarchy){
                     if(_hookcalclated == true){
                          if(_calcPower == true){
                               GetPowerValue();
                           }
                     }
                   }
               }
           };
          _gameactions.ButtonActions.driftbar.performed += y => {
               if(!_gamePaused && !_pauseMenupanel.activeInHierarchy){
              if(_canhit == true && _ball.activeInHierarchy)
                  {
                  if(_hookcalclated == false){
                      GetDriftValue();
                    _hookcalclated = true;
                   }
              }
         }
          };
        
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

            if (GameManager.instance._rankedMode)
            {
                if (_gameend && !_gameRankedFinished)
                {
                    CheckWinner();
                  
                }
            }
            if (_canhit == true)
            {
                if (_readyLunch)
                {
                    _myRocket?.GetComponent<GunfireController>().FireWeapon();
                    Waitstate();
                    _readyLunch = false;
                   
                }
                if(_hookcalclated ==false){

                    UpdateHookSlider();
                }else{
                       _hookScroll.gameObject.SetActive(false);
                }
                if(_hookcalclated == true && _powerval == false){
                 
                    UpdateGui();
                }
                if (!_usingRock)
                {
                    inputdir = new Vector3(_movingL.x, 0, 0);

                    transform.Translate(inputdir * Time.deltaTime);
                    Vector3 clampedPosition = transform.position;
                    clampedPosition.x = Mathf.Clamp(clampedPosition.x, _myxpos - 0.4f, _myxpos + 0.4f);
                    transform.position = clampedPosition;
                }
          
            }

        }else{

            if (_myManager == null)
            {
                PhotonNetwork.Destroy(_mypinsobj);
                PhotonNetwork.Destroy(this.gameObject);

            }
        }
       
       
    }
   
    private void BowlState()
    {
       if(!_gameend){
        _playercontext.Transition(_BowlingState);
       }
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
        _rankedPlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
        if (_rankedPlayers.Contains(this.gameObject))
        {
            _rankedPlayers.Remove(this.gameObject);
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
         
        if(_ControllPower == false){
              _slidertime += Time.deltaTime;
              _powerSlider.value = Mathf.Lerp(_powerSlider.minValue, _powerSlider.maxValue, _slidertime / 0.5f);
        }else{
         
              _slidertime += Time.deltaTime;
             _powerSlider.value = Mathf.Lerp(_powerSlider.maxValue, _powerSlider.minValue, _slidertime / 0.5f);
        }
        
         
    }
    IEnumerator ActivePowerShot(){

        yield return new WaitForSeconds(0.5f);
         _calcPower = true;
    }
    private void GetPowerValue(){
          _powerval = true;
           _power = _powerSlider.value;
          BowlState();
    }
    private void UpdateHookSlider()
    {
         if(_moveright == false){
           
             _scrolltime = 0; 
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 1f, _scrolltime /0.3f);
            
            if(_hookScroll.value >= 0.9){
                _moveright = true;
            }
         }else{
        _scrolltime = 0; 
            _scrolltime += Time.deltaTime;
            _hookScroll.value = Mathf.Lerp(_hookScroll.value, 0f, _scrolltime /0.3f);

            if(_hookScroll.value <= 0.1f){

                _moveright = false;
            }

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
         
           _hookScroll.gameObject.SetActive(false);
           _powerSlider.gameObject.SetActive(true);
           StartCoroutine(ActivePowerShot());
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
            if (_rankedPlayers.Count > 0 && !_rankedPanel.activeInHierarchy)
            {
                
                Debug.Log("Winner is ..... ");
                if (_rankedPlayers[0] != null)
                {
                    var findscore = _rankedPlayers[0].GetComponent<PlayerController>();
                    Debug.Log(_scoreplayer.totalscre);
                    Debug.Log(findscore._scoreplayer.totalscre);
                    if (findscore._gameend)
                    {
                        if (_scoreplayer.totalscre > findscore._scoreplayer.totalscre)
                        {
                            Debug.Log("you winn !!");
                            ShowRankedResult("win");
                        }
                        else if (_scoreplayer.totalscre < findscore._scoreplayer.totalscre)
                        {
                            Debug.Log("lose");
                            ShowRankedResult("lose");
                        }
                        else if (_scoreplayer.totalscre == findscore._scoreplayer.totalscre)
                        {
                            Debug.Log("Draw");
                            ShowRankedResult("draw");
                        }
                       
                        _gameRankedFinished = true;
                    }
                }
            }
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (GameManager.instance._rankedMode)
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
    public void Resume(){

        _pauseMenupanel.SetActive(false);
       StartCoroutine(WaitPause());
    }
    IEnumerator WaitPause(){
        yield return new WaitForSeconds(1);
        _gamePaused = false;
    }
    public void QuitGame(){

        if(PhotonNetwork.InRoom){
            
        PhotonNetwork.LeaveRoom();
        }
        
        PhotonNetwork.LoadLevel(0);
    }
    public void SoundOn(){
    
        AudioListener.volume = 1;
          EventSystem.current.SetSelectedGameObject(_soundOnOF[1]);

    }
     public void Soundoff(){
    
        AudioListener.volume = 0;
        EventSystem.current.SetSelectedGameObject(_soundOnOF[0]);
    }
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
                Debug.Log("Network Error");
            }
        }

      
    }
    public void ShowRankedResult(string state) 
    {
        if (_MyPlayCanavas.activeInHierarchy)
        {
            _MyPlayCanavas.SetActive(false);
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
                if (latpoints > 0)
                {
                    PlayerPrefs.SetInt("rankedpoints", latpoints - 1);
                }
                break;
            case "draw":
                _rankedstatetxt.text = "Draw !!";
                _rankedpointtxt.text = "+1 ";
                PlayerPrefs.SetInt("rankedpoints", latpoints + 1);
                break;
        }
        PlayerPrefs.Save();
        StartCoroutine(PostScore());
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

