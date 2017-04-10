using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    private InputField consoleInput;
    private RectTransform rectTransform;

    private bool consoleVisible = false;
    private float lastPressedButtonTime = 0;
	// Use this for initialization
	void Start ()
	{
	
	    GameObject find = GameObject.Find("ConsoleInput");
        consoleInput=find.GetComponent<InputField>();
	    consoleInput.enabled = false;
	    rectTransform = find.GetComponent<RectTransform>();
	    rectTransform.localScale = new Vector3(0,0,0);
	}

    // Update is called once per frame
	void Update ()
	{
	    bool keyUp = Input.GetKeyDown(KeyCode.N);

        if (keyUp)
	    {
	        float diff = Time.time   - lastPressedButtonTime;
	        if (diff > 0.3)
	        {
	            if (consoleVisible)
	            {
	                consoleInput.enabled = false;
	                consoleVisible = false;
                    rectTransform.localScale=new Vector3(0,0,0);
	            }
	            else
	            {
                    consoleInput.enabled = true;
                    consoleVisible = true;
                    rectTransform.localScale = new Vector3(1,1,1);

                }
            }
	        lastPressedButtonTime = Time.time;

	    }
	}
}
