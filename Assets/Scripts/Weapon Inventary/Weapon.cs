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
        protected float _lastShootTime;

        public PlayerAnimation.WeaponType holdingType;
        private GameObject weaponHolder;

        private Transform attackSpawnPosition;
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



        public override void Start()
        {
            InitWeaponHolder();



        }

        public void InitWeaponHolder()
        {
            Transform findDeepChild = transform.FindDeepChild("WeaponHolder");
            if (findDeepChild != null)
            {
                weaponHolder = findDeepChild.gameObject;

                if (_bulletSpawnPosition == null)
                {
                    attackSpawnPosition = transform.FindDeepChild("AttackSpawnPoint");
                    _bulletSpawnPosition = attackSpawnPosition;
                }
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

        public virtual void Attack()
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

            Quaternion rotation = BulletSpawnPosition.root.rotation;

            var bullet = (GameObject)Instantiate(
            BulletPrefab,
            BulletSpawnPosition.position,
            rotation);
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
            Transform holderParent = weaponHolder.transform.parent;
            weaponHolder.transform.SetParent(null);
            weaponHolder.GetComponent<MeshFilter>().sharedMesh = PickUpPrefab.GetComponent<MeshFilter>().sharedMesh;
            weaponHolder.GetComponent<MeshRenderer>().sharedMaterial = PickUpPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            weaponHolder.transform.localScale = PickUpPrefab.transform.localScale;           
            weaponHolder.transform.parent = holderParent;
            GetComponent<PlayerAnimation>().SetWeaponType(holdingType);
            attackSpawnPosition.localPosition = PickUpPrefab.transform.FindDeepChild("AttackSpawnPoint").transform.localPosition;
            _bulletSpawnPosition = attackSpawnPosition;

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
