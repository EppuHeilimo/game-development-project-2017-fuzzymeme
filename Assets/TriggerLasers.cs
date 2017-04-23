using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;

public class TriggerLasers : MonoBehaviour
{
    public GameObject HitPoint;
    public LayerMask hitlayer;
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30, hitlayer))
        {
            HitPoint.transform.position = hit.point;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Stats>().Damage(3);
        }
    }

    void OnParticleCollision(GameObject obj)
    {
        
    }
}
