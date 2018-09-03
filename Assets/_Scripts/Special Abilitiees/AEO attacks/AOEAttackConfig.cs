using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/AOE Attack"))]
    public class AOEAttackConfig : AbilityConfig
    {
        [Header("AOE Attack RANGE")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damageToEachTarget = 15f;

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToattachTo)
        {
            return objectToattachTo.AddComponent<AOEAttackBehaviour>();
        }
        public float GetAOEDamageToEachTarget()
        {
            return damageToEachTarget;
        }
        public float GetAOERadius()
        {
            return radius;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
