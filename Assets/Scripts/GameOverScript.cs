using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {



    private GameObject gameoverBackground;
    private Stats stats;
    private bool alive = true;
    private bool isBlack = false;
    private Image image;
    public float FadingOutTime = 5;
    public float ShowGameOverTime = 3;
    private GameObject player;

    // Use this for initialization
    void Start ()
	{
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<Stats>();

        gameoverBackground = GameObject.Find("GameOverBackground");
        image = gameoverBackground.GetComponent<Image>();

        Transform textTransform = gameoverBackground.transform.Find("Text");
        textTransform.gameObject.SetActive(false);
        //gameoverBackground.SetActive(false);


    }


    // Update is called once per frame
	void FixedUpdate () {
	    if (stats.CurrentLifeEnergy <= 0 && alive)
	    {
	        alive = false;

            Transform textTransform = gameoverBackground.transform.Find("Text");
            textTransform.gameObject.SetActive(true);
	        player.GetComponent<PlayerMovement>().locked = true;
            player.GetComponent<PlayerMovement>().animation.Die();


	    }
	}

    void Update()
    {

        if (!alive && !isBlack)
        {
            float alphaIncrement = Time.deltaTime* 1/FadingOutTime;
            alphaIncrement= image.color.a + alphaIncrement;
            if (alphaIncrement > 1)
            {
                alphaIncrement = 1;
                isBlack = false;
                StartCoroutine(EndGame());
            }
            image.color = new Color(0,0,0,alphaIncrement);
        }
        
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(ShowGameOverTime);
        // todo switch to main menu;
    }
}
