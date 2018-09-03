using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.PlayerCH;
namespace RPG.Core
{
    [ExecuteInEditMode]
    public class WeaponPickUpPoint : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip audioPickUp;
        // Use this for initialization
        AudioSource audioSource;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
          
        }
        void DestroyChildren()
        {
           foreach ( Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        
        }
        // Update is called once per frame
        void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
           Instantiate(weapon, gameObject.transform);
        }
        private void OnTriggerEnter(Collider other)
        {
            FindObjectOfType<Player>().PutWeaponInHand(weaponConfig);
            audioSource.PlayOneShot(audioPickUp);
        }
    }
}
