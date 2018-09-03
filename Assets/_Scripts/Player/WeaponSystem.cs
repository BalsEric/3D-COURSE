using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] GameObject weaponSocket;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;

        float lastHitTime;
        GameObject weaponObject;
        GameObject target;
        Animator animator;
        Character character;
        const string ATTACK_TRIGGER = "attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        void Start()
        {
            animator = GetComponent<Animator>();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
            character = GetComponent<Character>();
        }
        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            var animatorOverrideCOntroller= character.GetOverrideController();
            animator.runtimeAnimatorController = animatorOverrideCOntroller;
            animatorOverrideCOntroller[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimationClip(); // remove const
        }
        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
        }
        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER); // TODO make const

                lastHitTime = Time.time;
            }
        }
        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }
        private float CalculateDamage()
        {
           
            return baseDamage + currentWeaponConfig.GetAdditionalDamage();
            
            
        }
        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, weaponSocket.transform);

            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}