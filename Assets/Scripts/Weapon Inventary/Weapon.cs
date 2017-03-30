using Assets.Script;
using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public class Weapon : InventoryItem, IWeapon
    {

        public int Ammunition;
        public float ReloadTime = 0.5f;
        public GameObject BulletPrefab;
        private float _lastShootTime;

        public PlayerAnimation.WeaponType holdingType;
        private GameObject weaponHolder;
        private Transform _bulletSpawnPosition;
        public Transform BulletSpawnPosition
        {
            get
            {
                if (_bulletSpawnPosition == null)
                {
                    Vector3 spawnPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);

                    _bulletSpawnPosition = transform;
                }
                return _bulletSpawnPosition;
            }
        }



        void Start()
        {
            if (transform.CompareTag("Player"))
            {
                weaponHolder = transform.FindDeepChild("WeaponHolder").gameObject;
                Inventory inv = GetComponent<Inventory>();
                weaponHolder.GetComponent<MeshFilter>().sharedMesh = inv.Items[inv.Index].PickUpPrefab.GetComponent<MeshFilter>().sharedMesh;
                weaponHolder.GetComponent<MeshRenderer>().sharedMaterial = inv.Items[inv.Index].PickUpPrefab.GetComponent<MeshRenderer>().sharedMaterial;
                GetComponent<PlayerAnimation>().wepType = holdingType;
            }

            if (_bulletSpawnPosition == null)
            {
                _bulletSpawnPosition = transform.FindDeepChild("AttackSpawnPoint");
            }
        }

        public override int UseAbleAmount
        {
            get { return Ammunition; }
        }


        void Update()
        {


        }

        public override double ReloadPercentage
        {
            get
            {
                float passedTime = Time.time- _lastShootTime;
                if (passedTime > ReloadTime)
                {
                    return 1;
                }
                else
                {
                    float percentage = passedTime / ReloadTime;
                    Debug.Log(percentage);
                    return percentage;
                }
            }
        }

        public override string ItemDescription
        {
            get
            {
                return base.InventaryItemName+" | A: "+" | D:";
            }
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
            if (transform.CompareTag("Player") && holdingType == PlayerAnimation.WeaponType.Melee) 
            {
                GetComponent<PlayerAnimation>().AnimateSlash();
            }

            var bullet = (GameObject)Instantiate(
            BulletPrefab,
            BulletSpawnPosition.position,
            BulletSpawnPosition.root.rotation);
            Bullet component = bullet.GetComponent<Bullet>();
            if (Ammunition != -1)
            {
                Ammunition--;
            }
          
            _lastShootTime = Time.time;
            component.shooter = gameObject;

        }

        public override void OnBeingSelected()
        {
            _lastShootTime = Time.time;
            Inventory inv = GetComponent<Inventory>();
            weaponHolder.GetComponent<MeshFilter>().mesh = inv.Items[inv.Index].PickUpPrefab.GetComponent<MeshFilter>().sharedMesh;
            weaponHolder.GetComponent<MeshRenderer>().material = inv.Items[inv.Index].PickUpPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            GetComponent<PlayerAnimation>().SetWeaponType(holdingType);
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
            //weapon._bulletSpawnPosition = _bulletSpawnPosition;
            weapon.ReloadTime = ReloadTime;
            weapon._lastShootTime = _lastShootTime;
            weapon.InventaryItemName = InventaryItemName;
            weapon.holdingType = holdingType;
        }
    }


}
