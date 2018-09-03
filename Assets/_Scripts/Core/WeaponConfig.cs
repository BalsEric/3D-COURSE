using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("weapon"))]
    public class WeaponConfig : ScriptableObject
    {

        public Transform gripTransform;
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBwtweenHits = .5f;
        [SerializeField] float maxAttackRange = 3.5f;
        [SerializeField] float additionalDamage = 10f;
        public float GetMinTimeBetweenHits()
        {
            return minTimeBwtweenHits;
        }
        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }
        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }
        public AnimationClip GetAttackAnimClip()
        {
            attackAnimation.events = new AnimationEvent[0]; // removing animation events
            return attackAnimation;
        }
        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }


    }
   
}
