using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_CastAbility : BTNode
{
    [SerializeField] Ability abilityToCast;

    protected override BTNodeResult Execute()
    {
        GetOwner().GetComponent<AbilityComponent>().TryActiaveAbility(abilityToCast);
        return BTNodeResult.Success;
    }
}
