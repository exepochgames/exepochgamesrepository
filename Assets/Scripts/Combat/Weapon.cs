using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon", menuName ="RPG Project/New Weapon",order =0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverrider = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] bool isRightHanded = true;

        public void Spawn(Transform righthandTransform, Transform lefthandTransform, Animator animator)
        {
            if(equippedPrefab != null)
            {
                Transform handTransform;
                if (isRightHanded) handTransform = righthandTransform;
                else handTransform = lefthandTransform;
                Instantiate(equippedPrefab, handTransform);
            }
            if(animatorOverrider != null)
            {
                animator.runtimeAnimatorController = animatorOverrider;
            }
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetRange()
        {
            return weaponRange;
        }


    }
}