using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.EnemyCH;
// TODO consider re-wire...
using RPG.CameraUI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {
        
        EnemyAI enemy = null;
        WeaponSystem weaponSystem;
        SpecialAbilities abilities;
        CameraRaycaster cameraRaycaster=null;
       
        Character character;
       
        
        
        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChanse = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalhitParticle = null;
        void Start()
        {
            character = GetComponent<Character>();
            RegisterForMouseEvents();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
           
           
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
        void OnMouseOverEnemy(EnemyAI enemyToSet)
        {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
            }
        }
        
        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}