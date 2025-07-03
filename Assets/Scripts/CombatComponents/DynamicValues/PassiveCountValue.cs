using System;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PassiveCount", menuName = "DynamicValue/PassiveCount")]
public class PassiveCountValue : DynamicValue
{
    public TargetSelector entities;
    public PassiveDefinition passiveDef;
    public override int ComputeValue(Context context)
    {
        var targets = entities.GetTargets(context);
        var value = 0;
        foreach (var target in targets)
        {
            foreach (var passive in target.passives)
            {
                if(passive.definition == passiveDef)
                    value++;
            }
        }
        return value;
    }
}