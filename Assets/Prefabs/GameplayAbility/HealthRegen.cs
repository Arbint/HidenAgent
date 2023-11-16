using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability/Health Regen")]
public class HealthRegen : Ability
{
    [SerializeField] float regenAmount = 20f;
    [SerializeField] float regenDuration = 3f;

    public override void ActivateAbility()
    {
        HealthComponet healthComp = OwningAblityComponet.GetComponent<HealthComponet>();
        if (healthComp == null || healthComp.IsFull())
            return;

        if (!CommitAbility())
            return;

        StartCoroutine(HealthRegenCoroutine(healthComp));
    }

    IEnumerator HealthRegenCoroutine(HealthComponet healthComp)
    {
        float timeLeft = regenDuration;
        float regenRate = regenAmount / regenDuration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float deltaTime = Time.deltaTime;
            if (timeLeft < 0)
            {
                deltaTime += timeLeft;
            }
            healthComp.ChangeHealth(regenRate * deltaTime, OwningAblityComponet.gameObject);
            yield return new WaitForEndOfFrame();
        }
    }
}
