using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using System;

namespace RPG.EnemyCH
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypoinTolerance = 2f;
        float currentWeaponRange;
        enum State {idle,patroling,attacking,chase}
        State state = State.idle;
        PlayerMovement player = null;
        float distanceToPlayer;
        Character character;
        int nextWaypointIndex;
        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
            character = GetComponent<Character>();
        }
        private void Update()
        { 
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            if(distanceToPlayer > chaseRadius && state != State.patroling)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }
            if(distanceToPlayer <= chaseRadius && state !=State.chase)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if(distanceToPlayer <= currentWeaponRange && state !=State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
            }
        }
        IEnumerator ChasePlayer()
        {
            state = State.chase;
            while(distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator Patrol()
        {
            state = State.patroling;
            while(true)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointsWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void CycleWaypointsWhenClose(Vector3 nextWaypointPos)
        {
            if(Vector3.Distance(transform.position,nextWaypointPos) <= waypoinTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
         
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}

