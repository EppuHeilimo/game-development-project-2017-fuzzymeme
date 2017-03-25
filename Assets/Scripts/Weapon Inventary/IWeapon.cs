using UnityEngine;

namespace Assets.Scripts.Weapon_Inventary
{
    public interface IWeapon
    {

        void SetBulletSpawnPosition(Transform transform);
        void Attack();
    }
}