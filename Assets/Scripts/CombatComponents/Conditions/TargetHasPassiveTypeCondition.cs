using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetHasPassiveTypeCondition", menuName = "Conditions/TargetHasPassiveType")]
public class TargetHasPassiveTypeCondition : Condition
{
    public PassiveType passiveType;
    public override bool ShouldTrigger(Context context)
    {
        foreach (var passive in context.target.passives)
        {
            if (passive.definition.passiveTypes.Contains(passiveType))
            {
                return true;
            }
        }

        return false ;
    }
}