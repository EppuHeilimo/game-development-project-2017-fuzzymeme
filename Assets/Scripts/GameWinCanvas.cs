using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
