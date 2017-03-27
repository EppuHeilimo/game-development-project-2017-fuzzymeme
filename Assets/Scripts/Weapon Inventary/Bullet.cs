using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class Bullet : MonoBehaviour
    {
        public GameObject shooter;
        public float Damage;
        public float Speed;
        public float Distance;
        public Vector3 StartPosition;
        private float InitTime = 0.3f;
        private float InitTimer = 0f;
        private bool init = false;

        void Start()
        {
            InitTimer = Time.time;
            float time = Distance / Speed;
            Destroy(gameObject, time);
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
            
            if (collision.CompareTag("Player") && shooter.CompareTag("Player"))
            {
                if (Time.time - InitTimer > InitTime)
                {
                    Debug.Log(collision.gameObject.name);
                    Collision(collision);
                }
            }
            else if(collision.CompareTag("Bullet") || (collision.CompareTag("Enemy") && shooter.CompareTag("Enemy")))
            {
                
            }
            else
            {
                Collision(collision);
            }
            

        }

        void Collision(Collider collision)
        {
             
            GameObject gameobj1 = collision.gameObject;
            Stats component = gameobj1.GetComponent<Stats>();
            
            if (component != null)
            { 
                component.Damage(Damage);

            }
            Destroy(gameObject);
        }
    }
}
