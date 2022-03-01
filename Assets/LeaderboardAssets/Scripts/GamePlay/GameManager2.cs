using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // OnButtonSubmit
    public void OnButtonSubmit() {
        SceneManager.LoadSceneAsync("GameOver");
    }
}

