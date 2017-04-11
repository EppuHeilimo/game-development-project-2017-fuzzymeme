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
        private ParticleSystem bloodParticles;
        private bool notifiedZeroLifePoints = false;
        public void Start()
        {

            Transform t = transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == "BloodScript")
                {
                    bloodParticles = tr.GetComponent<ParticleSystem>();

                }
            }
 
        }

      

        private void Notify()
        {
            if(ZeroLifePoints != null && !notifiedZeroLifePoints)
            {
                ZeroLifePoints(this,EventArgs.Empty);
                notifiedZeroLifePoints = true;
            }

        }

        public void Damage(float damage)
        {
            if (transform.CompareTag("Enemy"))
            {
                GetComponent<AI>().GotAttacked();
            }

            var hasBloodParticles = bloodParticles != null;
            if (hasBloodParticles)
            {
                bloodParticles.Play(true);
                bloodParticles.enableEmission = true;
            }
            


      
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

        public void Heal(double healingPoints)
        {
            double newCurrentLifeEnergy = CurrentLifeEnergy+healingPoints;
            if (newCurrentLifeEnergy > LifeEnergy)
            {
                newCurrentLifeEnergy = LifeEnergy;
            }
            CurrentLifeEnergy = newCurrentLifeEnergy;
        }
    }
}
