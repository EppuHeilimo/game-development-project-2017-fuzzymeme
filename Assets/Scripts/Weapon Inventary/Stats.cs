using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapon_Inventary
{
    public class Stats : MonoBehaviour, IZeroLifePointNotify
    {


        public Text Text;
        public String CreatureName;


        public double LifeEnergy;
        public double CurrentLifeEnergy;

        public event EventHandler ZeroLifePoints;

        private void Notify()
        {
            if(ZeroLifePoints != null)
            {
                ZeroLifePoints(this,EventArgs.Empty);
            }
        }

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
            if(CurrentLifeEnergy == 0)
            {
                Notify();
            }
            if (Text != null)
                Text.text = CreatureName + ":  " + CurrentLifeEnergy + " / " + LifeEnergy;

        }
    }
}
