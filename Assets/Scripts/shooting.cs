using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour {


    
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed;
    public float shootTime;
    public float interval;
   
	


	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        if(shootTime > 0 ){
            shootTime -= Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.Mouse0) && shootTime < 0)
        {

            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

            shootTime += interval;
            Destroy(bullet, 5.0f);
            //Debug.Log(shootTime);
            
      
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("isHit"))
        {
            other.gameObject.SetActive(false);


        }

    }
}
