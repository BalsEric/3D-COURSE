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
        [SerializeField] float animationCycle = 0.5f;
        [SerializeField] float maxAttackRange = 3.5f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] float damageDelay = .5f;
       
        public float GetMinTimeBetweenHits()
        {
            return animationCycle;
        }
        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }
        public float GetDamageDelay()
        {
            return damageDelay;
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
