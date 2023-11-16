using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    HealthComponet healthComponet;
    PlayerValueGuage valueGuage;
    private void Start()
    {
        GameObject playerGameObject = GameplayStatics.GetPlayerGameObject();
        healthComponet = playerGameObject.GetComponent<HealthComponet>();
        
        valueGuage = GetComponent<PlayerValueGuage>();
        valueGuage.SetValue(healthComponet.GetHealth(), healthComponet.GetMaxHealth());
        healthComponet.onHealthChanged += UpdateValue;
    }

    private void UpdateValue(float currentHealth, float delta, float maxHealth)
    {
        valueGuage.SetValue(currentHealth, maxHealth);
    }
}
