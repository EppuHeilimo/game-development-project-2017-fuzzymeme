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

        void Start()
        {

            float time = Distance / Speed;
            Destroy(gameObject, time);
        }

        void Update()
        {
        
            Vector3 movement = Time.deltaTime * Speed * transform.forward;
            transform.position = transform.position - movement;

        }

        void OnCollisionEnter(Collision collision)
        {
            GameObject gameobj1 = collision.gameObject;

            if (shooter == gameobj1)
            {
                return;
            }

            Stats component = gameobj1.GetComponent<Stats>();
            if (component != null)
            {

                component.Damage(Damage);

            }
            Destroy(gameObject);

        }
    }
}
