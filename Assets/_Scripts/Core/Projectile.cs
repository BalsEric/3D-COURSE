using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed;
        public float damageCause { get; set; }
        // Use this for initialization
        const float DESTROY_DELAY = 0.1f;
        [SerializeField] GameObject shooter;
        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }
        void OnCollisionEnter(Collision collision)
        {
            var LayerCollidedWith = collision.gameObject.layer;
           
            if (shooter && LayerCollidedWith != shooter.layer)
            {
               // DamageIfDamagable(collision);
            }
        }
        //private void DamageIfDamagable(Collision collision)
        //{
        //    Component damagableComponent = collision.gameObject.GetComponent(typeof(IDamagable));

        //    if (damagableComponent)
        //    {
        //        (damagableComponent as IDamagable).TakeDamage(damageCause);


        //    }
        //    Destroy(gameObject, DESTROY_DELAY);
        //}

        private void SetDamage(float damage)
        {
            damageCause = damage;
        }
        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }
    }
}
