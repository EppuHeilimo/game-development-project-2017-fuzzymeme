using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class Weapon : InventoryItem, IWeapon
    {

        public int Ammunition;
        public float ReloadTime = 0.5f;
        public GameObject BulletPrefab;
        public Transform BulletSpawnPosition;


        private float _lastShootTime;

        void Start()
        {
            if(BulletSpawnPosition == null)
            {
                BulletSpawnPosition = transform.FindChild("BulletSpawnPoint");
            }
        }

        public void SetBulletSpawnPosition(Transform transform)
        {
            BulletSpawnPosition = transform;
        }

        public override int UseAbleAmount
        {
            get { return Ammunition; }
        }


        void Update()
        {


        }

        public void Attack()
        {

            if (Ammunition == 0)
            {
                return;
            }
            if (Time.time - _lastShootTime < ReloadTime)
            {

                return;

            }


            var bullet = (GameObject)Instantiate(
            BulletPrefab,
            BulletSpawnPosition.position,
            BulletSpawnPosition.rotation);
            Bullet component = bullet.GetComponent<Bullet>();
            Ammunition--;
            _lastShootTime = Time.time;
            component.shooter = gameObject;

        }


        public override void Use()
        {
            Attack();
        }


        protected override void OnCreateCopy(InventoryItem addComponent)
        {
            Weapon weapon = addComponent as Weapon;
            weapon.Ammunition = Ammunition;
            weapon.BulletPrefab = BulletPrefab;
            weapon.BulletSpawnPosition = BulletSpawnPosition;
            weapon.ReloadTime = ReloadTime;
            weapon._lastShootTime = _lastShootTime;
            weapon.InventaryItemName = InventaryItemName;
        }
    }


}
