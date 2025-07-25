
using System;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PercentHpValue", menuName = "DynamicValue/PercentHpValue")]
public class PercentHpValue : DynamicValue
{
    public TargetSelector targetSelector;
    protected override int ComputeValue(Context context)
    {
        var targets = targetSelector.GetTargets(context);
        if (targets.Count != 1)
        {
            throw new ArgumentException("target count is invalid, need 1, found " + targets.Count);
        }

        return Mathf.RoundToInt((float)targets[0].GetCarac(Carac.CurrentHp)*100/targets[0].GetCarac(Carac.MaxHp));
    }
}