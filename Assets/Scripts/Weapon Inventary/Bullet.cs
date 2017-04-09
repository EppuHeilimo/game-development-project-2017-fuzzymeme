using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class Bullet : MonoBehaviour
    {
        public GameObject shooter;
        public float Damage;
        public float Speed;
        public float Distance;
        private float InitTime = 0.3f;
        private float InitTimer = 0f;
        private bool init = false;

        void Start()
        {
            InitTimer = Time.time;
            float time = Distance / Speed;
            Destroy(gameObject, time);
            gameObject.tag = "Bullet";
        }

        void Update()
        {
            InitTimer += Time.deltaTime;
            if(InitTimer > InitTime)
            {

            }
            Vector3 movement = Time.deltaTime * Speed * transform.forward;
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
                else if (collision.CompareTag("isHit") || (collision.CompareTag("Enemy") && shooter.CompareTag("Enemy")))
                {
                    // do nothing
                }else if (collision.gameObject.CompareTag("Bullet"))
                {
                    //do nothing
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
            Destroy(gameObject);
        }
    }
}
