
using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PercentHpValue", menuName = "DynamicValue/PercentHpValue")]
public class PercentHpValue : DynamicValue
{
    public TargetSelector targetSelector;
    public override int computeValue(Context context)
    {
        var targets = targetSelector.GetTargets(context);
        if (targets.Count != 1)
        {
            throw new ArgumentException("target count is invalid, need 1, found " + targets.Count);
        }

        return Mathf.RoundToInt((float)targets[0].GetCarac(Carac.currentHp)*100/targets[0].GetCarac(Carac.maxHp));
    }
}