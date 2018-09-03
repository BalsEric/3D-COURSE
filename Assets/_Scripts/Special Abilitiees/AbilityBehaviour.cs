using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{

    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;
        const float PART_CLEAN_UP_DELAY = 20f;
        public abstract void Use(GameObject target = null);
        const string ATTACK_TRIGGER = "attack";
        const string DEFAULT_ATTACK_STATE = "DEFAULT_ATTACK";
        public void SetConfig(AbilityConfig configToSet)
        {
           config = configToSet;
        }
        protected void PlayParticleEfect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            particleObject.transform.parent = transform;
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));

        }
        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while(particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PART_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }
        protected void PlayAbilityAnimation()
        {
            var animatorOverrideController = GetComponent<Character>().GetOverrideController();
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK_STATE] = config.GetAnimationClip();
            animator.SetTrigger(ATTACK_TRIGGER);
        }
        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }
    }
}

