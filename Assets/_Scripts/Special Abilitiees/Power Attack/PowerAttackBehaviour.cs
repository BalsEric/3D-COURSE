using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.PlayerCH
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
            PlayParticleEfect();
            PlayAbilitySound();
        }
        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}
