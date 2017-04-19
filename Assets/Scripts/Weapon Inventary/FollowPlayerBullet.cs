using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class FollowPlayerBullet : Bullet
    {

        private Transform playerTransform;

        void Start()
        {
            OnStart();


            GameObject player = GameObject.Find("Player");
            playerTransform = player.transform;
        }


        void Update()
        {
            Vector3 vector3 = playerTransform.position - transform.localPosition;
            vector3.y = 0;
            vector3.Normalize();

            Vector3 vector4 = vector3* Time.deltaTime* Speed;
            transform.position = transform.position+ vector4;
        }
    }
}
