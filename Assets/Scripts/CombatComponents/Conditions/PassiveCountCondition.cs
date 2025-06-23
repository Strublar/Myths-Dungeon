using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveCountCondition", menuName = "Conditions/PassiveCount")]
public class PassiveCountCondition : Condition
{

    public int count = 1;
    public PassiveDefinition passive;
    public TargetSelector holder;
    public override bool ShouldTrigger(Context context)
    {
        int passiveCount = 0;
        foreach (var p in holder.GetTargets(context)[0].passives)
        {
            if (p.definition == passive)
                passiveCount++;
        }
        return passiveCount >= count;
    }
}