using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapon_Inventary
{
    public class Stats : MonoBehaviour
    {


        public Text Text;
        public String CreatureName;


        public double LifeEnergy;
        public double CurrentLifeEnergy;
        public void Damage(float damage)
        {
            double newLifeEnergy = CurrentLifeEnergy - damage;
            if (newLifeEnergy <= 0)
            {
                CurrentLifeEnergy = 0;
            }
            else
            {
                CurrentLifeEnergy = newLifeEnergy;
            }

            if (Text != null)
                Text.text = CreatureName + ":  " + CurrentLifeEnergy + " / " + LifeEnergy;

        }




    }
}
