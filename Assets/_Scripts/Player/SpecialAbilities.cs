﻿using UnityEngine;
using UnityEngine.UI;
using RPG.EnemyCH;
namespace RPG.PlayerCH
{
   
    public class SpecialAbilities : MonoBehaviour
    {
       
        [SerializeField] float maxEnergyPoints=100f;
        [SerializeField] float regenPointsPerSecond = 5f;
        [SerializeField] Image energyBar;
        float currentEnergyPoints;
        AudioSource audioSource;
        [SerializeField] AbilityConfig[] abilities;
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
            UpdateEnergyBar();
            audioSource = GetComponent<AudioSource>();
        }
        
        void Update()
        {
            if(currentEnergyPoints<maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }
        public int GetNumerOfAbilities()
        {
            return abilities.Length;
        }
        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }
        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }
        public void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }
        public void AttemptSpecialAbility(int abilityIndex)
        {
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyCost<= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
            }
            else
            {
                //
            }

        }
        public void UpdateEnergyBar()
        {
            energyBar.fillAmount = EnergyAsPercent;
        }
        float EnergyAsPercent
        {
            get{
                return currentEnergyPoints / maxEnergyPoints;
            }
           
        }
    }
}