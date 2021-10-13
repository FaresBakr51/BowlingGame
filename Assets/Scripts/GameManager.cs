using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject _leaderBoardobj;
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if(_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GameManager).Name;
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as GameManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RequestLeaderBoard()
    {
        if (_leaderBoardobj == null)
        {
            _leaderBoardobj = GameObject.FindWithTag("leaderboard");
        }
        StartCoroutine(Waitleader());
    }
    public void OfflineMode()
    {
        GetScene(1);
    }
    private void Update()
    {
        if (Input.GetButtonDown("square"))
        {
            GetScene(1);
        }
    }
    private void GetScene(int sceneindx)
    {
        SceneManager.LoadScene(sceneindx);
    }
    IEnumerator Waitleader()
    {
        _leaderBoardobj.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        _leaderBoardobj.transform.GetChild(0).gameObject.SetActive(false);
    }
}

