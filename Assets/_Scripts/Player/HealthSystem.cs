using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace RPG.PlayerCH
{

    public class HealthSystem : MonoBehaviour {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthbar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        const string DEATH_TRIGGER = "death";
        [SerializeField] float deathVanishSeconds = 1f;
        float currentHealthPoints = 0;
        Animator animator;
        AudioSource audioSource;
        Character characterMovement;
         public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }
        // Use this for initialization
        void Start() {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();
            currentHealthPoints = maxHealthPoints;
        }

        // Update is called once per frame
        void Update() {
            UpdateHealthBar();
        }
        void UpdateHealthBar()
        {
            if ( healthbar)
            {
                healthbar.fillAmount = healthAsPercentage;
            }
        }
        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoints - damage) <= 0;
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);
            if (characterDies) 
            {
                StartCoroutine(KillCharacter());
               
            }
        }
        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }
        IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);
            var playerComponment = GetComponent<PlayerMovement>();
            if(playerComponment && playerComponment.isActiveAndEnabled)
            {
                audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                audioSource.Play();
                yield return new WaitForSecondsRealtime(audioSource.clip.length);
                SceneManager.LoadScene(0);
            }
            else
            {
                Destroy(gameObject, deathVanishSeconds);
            }
        }
    }
}
