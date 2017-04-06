﻿using System;
using Assets.Script;
using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class MultiBulletWeapon : Weapon
    {



        public int Number = 5;
        public float AngleBetweenBullets= 12;
   

     
        public override void Attack()
        {

            if (Ammunition == 0)
            {
                return;
            }
            if (Time.time - _lastShootTime < ReloadTime)
            {

                return;

            }
            if (transform.CompareTag("Player") && holdingType == PlayerAnimation.WeaponType.Melee) 
            {
                GetComponent<PlayerAnimation>().AnimateSlash();
            }

            float startPosition;
            bool evenNumber = Number%2 == 0;
            if (evenNumber)
            {
                startPosition = -((AngleBetweenBullets)*(Number/2.0f));
                startPosition = startPosition - AngleBetweenBullets / 2;

            }
            else
            {
                startPosition = -((AngleBetweenBullets) * (Number / 2.0f));

            }


            for (int i = 0; i < Number; i++)
            {
                float y = startPosition+AngleBetweenBullets*i;
              
                var bullet = (GameObject)Instantiate(
                BulletPrefab,
                BulletSpawnPosition.position,
                BulletSpawnPosition.root.rotation);

                bullet.transform.Rotate(0,y,0,Space.World);
                Bullet component = bullet.GetComponent<Bullet>();

                component.shooter = gameObject;

            }


            if (Ammunition != -1)
            {
                Ammunition--;
            }
          
            _lastShootTime = Time.time;

        }




        protected override void OnCreateCopy(InventoryItem addComponent)
        {
            base.OnCreateCopy(addComponent);

        }
    }


}