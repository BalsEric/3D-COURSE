﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.PlayerCH
{
    [CreateAssetMenu(menuName =("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : AbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float extraDamage = 10f;
        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToattachTo)
        {
            return objectToattachTo.AddComponent<PowerAttackBehaviour>();
        }
        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }
}