using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wire...
using RPG.CameraUI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
        
       
        WeaponSystem weaponSystem;
        SpecialAbilities abilities;
       
        Character character;
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
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMousePotentionallyWalkable;
         
        }
        void OnMouseOverEnemy(EnemyAI enemy)
        {
            
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject))
            {
                abilities.AttemptSpecialAbility(0,enemy.gameObject);

            }
            else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }
        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            while(!IsTargetInRange(target))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }
        IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
        }

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}