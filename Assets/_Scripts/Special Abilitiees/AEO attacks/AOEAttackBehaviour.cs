﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.PlayerCH
{
    public class AOEAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            DealRadialDamage();
            PlayParticleEfect();
            PlayAbilitySound();
        }
        private void DealRadialDamage()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, (config as AOEAttackConfig).GetAOERadius(), Vector3.up, (config as AOEAttackConfig).GetAOERadius());
            float damageToDeal = (config as AOEAttackConfig).GetAOEDamageToEachTarget();
            foreach (RaycastHit hit in hits)
            {
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                var damagable = hit.collider.gameObject.GetComponent<HealthSystem>();
                if (damagable != null && !hitPlayer)
                {
                    damagable.TakeDamage(damageToDeal);
                }
            }
        }
       
    }
}
