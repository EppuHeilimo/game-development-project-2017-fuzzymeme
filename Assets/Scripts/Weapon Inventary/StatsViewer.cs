using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Weapon_Inventary
{
    public class StatsViewer : MonoBehaviour
    {

        private Image image;
        private Stats stats;
        private float width;

        public void Start()
        {
            image = GameObject.Find("LifeBar/LifeBarEmpty/LifebarFull").GetComponent<Image>();
            width=image.rectTransform.rect.width;
            stats = transform.GetComponent<Stats>();
        }

        private double lastValue = -1;
        public void Update()
        {
            if (Math.Abs(lastValue - stats.CurrentLifeEnergy) < Double.Epsilon)
            {
                return;
            }
            else
            {
                double percentage = stats.CurrentLifeEnergy/stats.LifeEnergy;

                width = (float)(width * percentage);
                image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

                lastValue = stats.CurrentLifeEnergy;
            }
        }
    }
}
