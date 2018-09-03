using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.PlayerCH
{
    public class SelfHealBehaviour: AbilityBehaviour
    {
        Player player=null;
       
       
        public override void Use(AbilityUseParams useParams)
        {
            PlayParticleEfect();
            var playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayAbilitySound();

        }
        void Start()
        {
            player = GetComponent<Player>();
          
        }
    }
}
