using System;
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
        public void SetAttackAnimation()
        {
            Debug.Log("SET ATTACK ANIMATION");
            if(!character.GetOverrideController())
            {
                Debug.Log(" NOT OVERRIDE");
            }
            else
            {
                Debug.Log("OVERRIDE 1");
                var animatorOverrideCOntroller= character.GetOverrideController();
                Debug.Log("OVERRIDE 2");
                animator.runtimeAnimatorController = animatorOverrideCOntroller;
                Debug.Log("OVERRIDE 3");
                animatorOverrideCOntroller[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimationClip();
                Debug.Log("OVERRIDE 4");
            }
            Debug.Log("OVERRIDE 5");

        }
        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }
        IEnumerator AttackTargetRepeatedly()
        {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            while(attackerStillAlive && targetStillAlive)
            {
                float weaponHitPeriod = currentWeaponConfig.GetMinTimeBetweenHits();
                float timeToWait = weaponHitPeriod * character.GetAnimationSpeedMultiplier();
                bool isTimeToHitAgain = (Time.time - lastHitTime) > timeToWait;
                if(isTimeToHitAgain)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
            

        }

        void AttackTargetOnce()
        {
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = 1f;
            SetAttackAnimation();
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
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