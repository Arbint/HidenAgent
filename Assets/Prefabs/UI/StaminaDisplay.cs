using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaDisplay : MonoBehaviour
{
    PlayerValueGuage valueGuage;
    private void Start()
    {
        GameObject playerGameObject = GameplayStatics.GetPlayerGameObject();
        AbilityComponent abilityComp = playerGameObject.GetComponent<AbilityComponent>();
        abilityComp.onStaminaChanged += StaminaChange;
        valueGuage = GetComponent<PlayerValueGuage>();
        valueGuage.SetValue(abilityComp.GetStamina(), abilityComp.GetMaxStamina());
    }

    private void StaminaChange(float currentValue, float maxValue)
    {
        valueGuage.SetValue(currentValue, maxValue);
    }
}
