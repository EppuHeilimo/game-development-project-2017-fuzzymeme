
using UnityEngine;

public class LaserEyes : MonoBehaviour
{
    public GameObject Eye1;
    public GameObject Eye2;
    private Transform player;
    public float speed = 5f;

    // Use this for initialization
    void Start () {
        CloseEyes();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update ()
	{

	    Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
	    transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, speed*Time.deltaTime);
        //keep only x rotation, everything else is coming from parent object
        transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
	}

    public void OpenEyes()
    {
        Eye1.SetActive(true);
        Eye2.SetActive(true);
    }

    public void CloseEyes()
    {
        Eye1.SetActive(false);
        Eye2.SetActive(false);
    }
}
