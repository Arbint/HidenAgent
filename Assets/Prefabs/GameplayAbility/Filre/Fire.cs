using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Fire")]
public class Fire : Ability
{
    [SerializeField] TargetScaner scanerPrefab;
    [SerializeField] float range = 3;
    [SerializeField] float scanDuration = 0.5f;

    [SerializeField] float damage = 40f;
    [SerializeField] float damageDuration = 3f;

    [SerializeField] GameObject ScanVFX;
    TargetScaner scaner;
    public override void ActivateAbility()
    {
        if (!CommitAbility())
            return;

        scaner = Instantiate(scanerPrefab);
        scaner.Init( range, scanDuration, ScanVFX == null ? null : Instantiate(ScanVFX) );
        scaner.SetupAttachment(OwningAblityComponet.gameObject.transform);
        scaner.StartScan();
        scaner.onNewTargetFound += ApplyDamage;
    }

    private void ApplyDamage(GameObject target)
    {
        HealthComponet targetHealthComp = target.GetComponent<HealthComponet>();
        if (targetHealthComp == null)
            return;

        if (target.GetComponent<ITeamInterface>().GetRelationTowards(OwningAblityComponet.gameObject) != TeamRelation.Hostile)
            return;

        DurationDamager damager = targetHealthComp.gameObject.AddComponent<DurationDamager>();
        damager.Init(damageDuration, damage, targetHealthComp, OwningAblityComponet.gameObject);
    }
}
