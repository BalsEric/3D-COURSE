using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core
{
    [CreateAssetMenu(menuName = ("weapon"))]
    public class Weapon : ScriptableObject
    {

        public Transform gripTransform;
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBwtweenHits = .5f;
        [SerializeField] float maxAttackRange = 2f;
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
        public AnimationClip GetAttackAnimationClip()
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
