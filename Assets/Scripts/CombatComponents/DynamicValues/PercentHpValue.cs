
using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewDynamicValue", menuName = "DynamicValue/PercentHpValue")]
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

        return Mathf.RoundToInt((float)targets[0].currentHp*100/targets[0].maxHp);
    }
}