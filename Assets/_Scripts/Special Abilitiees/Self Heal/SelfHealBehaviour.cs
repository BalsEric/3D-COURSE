using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour: AbilityBehaviour
    {
        PlayerMovement player=null;
       
       
        public override void Use(GameObject target)
        {
            PlayParticleEfect();
            var playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayAbilitySound();

        }
        void Start()
        {
            player = GetComponent<PlayerMovement>();
          
        }
    }
}
