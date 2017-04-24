using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class Bullet : MonoBehaviour, IPoolAble
    {
        private bool isInactive = true;


        public GameObject shooter;
        public float Damage;
        public float Speed;
        public float Distance;
        private float InitTime = 0.3f;
        private float InitTimer = 0f;
        private bool init = false;
        private float lifeTime;
        private int hashCode;
        public Vector3 spawnPosition;


        public void SetActive()
        {
            IsInActive = false;
            gameObject.SetActive(true);
            StartCoroutine(DestroyTimer());
            InitTimer = Time.time;


        }


        void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {
            lifeTime = Distance / Speed;
            gameObject.tag = "Bullet";
        }

        IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(lifeTime);
            SetInActive();

        }

        void  Update()
        {
            Vector3 direction = transform.forward;
            //boss shoots downwards towards the player.
            if (shooter.transform.CompareTag("Boss") && transform.position.y > 0.5f)
            {
                Vector3 heading = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 1, 0) - spawnPosition;
                             
                direction.y = (heading / heading.magnitude).y;
            }
            Vector3 movement = Time.deltaTime*Speed*direction;

            transform.position = transform.position + movement;
        }

        void OnTriggerEnter(Collider collision)
        {
            GameObject target = collision.gameObject;
            if (shooter != null)
            {
                if (shooter.CompareTag("Player") && target == shooter)
                {
                    if (Time.time - InitTimer > InitTime)
                    {
                        Collision(collision);
                    }
                }
                else if (collision.CompareTag("isHit") || (collision.CompareTag("Enemy") && shooter.CompareTag("Enemy")) || collision.CompareTag("InventoryItem"))
                {
                    // do nothing
                }else if (collision.gameObject.CompareTag("Bullet"))
                {
                    //do nothing
                }
                else if (collision.gameObject.CompareTag("InventoryItem"))
                {
                    //do nothing
                }
                else if (collision.CompareTag("Boss") && (shooter.CompareTag("Boss") || shooter.CompareTag("Minion")))
                {
                    //do nothing
                }
                else if (collision.CompareTag("Minion") && (shooter.CompareTag("Minion") || shooter.CompareTag("Boss")))
                {

                }
                else
                {
                    Collision(collision);
                }
            }
            else
            {
                Collision(collision);
            }

        }

        void Collision(Collider collision)
        {
            GameObject gameobj1 = collision.gameObject;
            Stats stats = gameobj1.GetComponent<Stats>();
            if (stats != null)
            { 
                stats.Damage(Damage);
            }
            SetInActive();
        }

        public bool IsInActive { get; private set; }
        public event EventHandler<PoolAbleEventArgs> Inactivated;

       
        public void SetInActive()
        {
            IsInActive = true;
            gameObject.SetActive(false);
            OnInactivated(new PoolAbleEventArgs(hashCode));
        }

        public void Init(int hashCode)
        {
            this.hashCode = hashCode;
            gameObject.SetActive(false);
        }

        protected virtual void OnInactivated(PoolAbleEventArgs e)
        {
            var handler = Inactivated;
            if (handler != null) handler(this, e);
        }
    }
}
