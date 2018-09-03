using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.EnemyCH;
// TODO consider re-wire...
using RPG.CameraUI;
using RPG.Core;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace RPG.PlayerCH
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Stats")]
       
        [SerializeField] float baseDamage = 10f;
        [Header("Weapon")]
        [SerializeField] Weapon currentWeaponInUse = null;
        [SerializeField] GameObject weaponSocket;
        GameObject weaponObject;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
       
        Enemy enemy = null;

        SpecialAbilities abilities;
        CameraRaycaster cameraRaycaster=null;
        float lastHitTime = 0f;
        Character character;
        const string ATTACK_TRIGGER = "attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        Animator animator;
        [Header("Critical")]
        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChanse = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [Header("Particle System")]
        [SerializeField] ParticleSystem criticalhitParticle = null;
        void Start()
        {
            character = GetComponent<Character>();
            RegisterForMouseEvents();
            abilities = GetComponent<SpecialAbilities>();
            PutWeaponInHand(currentWeaponInUse);
            SetAttackAnimation();
           
        }
        void Update()
        {
           ScanForAbilityKeyDown();
        }
        void ScanForAbilityKeyDown()
        {
            for(int keyIndex=1;keyIndex<=abilities.GetNumerOfAbilities();keyIndex++)
            {
                if(Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }
        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponInUse.GetAttackAnimationClip(); // remove const
        }
       void OnMousePotentionallyWalkable(Vector3 destionation)
        {
            if(Input.GetMouseButton(0))
            {
                character.SetDestination(destionation);
            }
        }
       
        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMousePotentionallyWalkable;
         
        }
        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                AttackTarget();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
            }
        }
        
        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponInUse.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER); // TODO make const
                
                lastHitTime = Time.time;
            }
        }
        public void PutWeaponInHand(Weapon weaponToUse)
        {
            currentWeaponInUse = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            Destroy(weaponObject);
            weaponObject= Instantiate(weaponPrefab, weaponSocket.transform);

            weaponObject.transform.localPosition = currentWeaponInUse.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponInUse.gripTransform.localRotation;
        }
        private float CalculateDamage()
        {
            float critChanse = UnityEngine.Random.value;
            float damageBeforeCrit = (baseDamage + currentWeaponInUse.GetAdditionalDamage());
            if (critChanse < criticalHitChanse)
            {
                Console.WriteLine("CRITICAL");
                criticalhitParticle.Play();
                return damageBeforeCrit * criticalHitMultiplier;
            }
            return damageBeforeCrit;
        }
        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeaponInUse.GetMaxAttackRange();
        }
    }
}