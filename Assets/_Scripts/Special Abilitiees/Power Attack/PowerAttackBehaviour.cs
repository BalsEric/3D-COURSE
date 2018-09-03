﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.PlayerCH
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            DealDamage(target);
            PlayParticleEfect();
            PlayAbilitySound();
        }
        private void DealDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}
