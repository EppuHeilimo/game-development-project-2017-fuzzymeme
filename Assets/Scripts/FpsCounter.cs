using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FpsCounter : MonoBehaviour
{
    private Text fpsText;
    private float timer;
    private int count = 0;
    private float deltaTimeAvarage = 0f;
	// Use this for initialization
	void Start ()
	{
	    fpsText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        deltaTimeAvarage += (Time.deltaTime - deltaTimeAvarage) * 0.1f;
	    timer += Time.deltaTime;
	    if (timer > 1f)
	    {
	        timer = 0;
            fpsText.text = Mathf.FloorToInt(1f / deltaTimeAvarage).ToString();
        }
	    



	}
}
