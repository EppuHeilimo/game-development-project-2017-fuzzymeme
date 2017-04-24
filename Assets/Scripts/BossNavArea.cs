using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;

public class BossNavArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        other.GetComponent<Stats>().Damage(1);
    }
}
