using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyAfterZeroLifePoints : MonoBehaviour
    {
        private ParticleSystem blood;
        private Stats Stats { get; set; }

        void Start()
        {
            Stats = GetComponent<Stats>();
             blood = GetComponent<ParticleSystem>();
        }

        void Update()
        {

            if (Math.Abs(Stats.CurrentLifeEnergy) < Double.Epsilon)
            {


                Transform t = transform;
                foreach (Transform tr in t)
                {
                    if (tr.tag == "BloodScript")
                    {
                        var bloodParticles = tr.GetComponent<ParticleSystem>();
                        bloodParticles.transform.parent = null;

                    }
                }

                //var component = gameObject.GetComponent<AI>();
                //Destroy(component);
                //BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
                //Destroy(boxCollider);
              
                //foreach (Transform o in transform)
                //{

                //    if(!o.CompareTag("BloodScript"))
                //    {
                //        Destroy(o);
                //    }   
                //}

                Destroy(gameObject);
            }
        }




    }
}
