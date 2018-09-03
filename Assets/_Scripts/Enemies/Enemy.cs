using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.PlayerCH;
using RPG.Core;
namespace RPG.EnemyCH
{
    public class Enemy : MonoBehaviour,IDamagable
    {

       
       
      
        [SerializeField] float attackRadius = 3f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float firingPeriodInS = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] Vector3 aimOffSet = new Vector3(0, 1f, 0);
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projetileSocket;
        bool IsAttacking = false;
        Player player = null;
       
        private void Start()
        {
            player = FindObjectOfType<Player>();
            
        }
        public void TakeDamage(float amount)
        {
            
        }
        private void Update()
        {
            
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !IsAttacking)
            {
                IsAttacking = true;
                float randomisedDelay =Random.Range(firingPeriodInS - firingPeriodVariation, firingPeriodInS + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay);

            }
            if (distanceToPlayer >= attackRadius)
            {
                CancelInvoke();
                IsAttacking = false;
               
            }


            if (distanceToPlayer <= chaseRadius)
            {
               // aICharacterControl.SetTarget(player.transform);
                
            }
            else
            {
                //aICharacterControl.SetTarget(transform);
              
            }



        }

        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projetileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.damageCause = damagePerShot;
            projectileComponent.SetShooter(gameObject);
            Vector3 unitVectorToPlayer = (player.transform.position + aimOffSet - projetileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

        }



    }
}

