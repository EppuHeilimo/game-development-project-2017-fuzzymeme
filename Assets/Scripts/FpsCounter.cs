using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FpsCounter : MonoBehaviour
{
    private Text fpsText;
    private float timer;
    private int count = 0;
    private float deltaTime = 0f;
	// Use this for initialization
	void Start ()
	{
	    fpsText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;


	    fpsText.text = Mathf.FloorToInt(1f/deltaTime).ToString();



	}
}
