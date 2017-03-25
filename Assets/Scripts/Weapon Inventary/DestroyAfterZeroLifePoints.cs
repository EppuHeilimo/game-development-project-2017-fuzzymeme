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

        public Stats Stats;


        void Update()
        {
            if (Math.Abs(Stats.CurrentLifeEnergy) < Double.Epsilon)
            {
                Destroy(gameObject);
            }
        }




    }
}
