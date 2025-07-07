using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PercentDynamicValue", menuName = "DynamicValue/PercentDynamicValue")]
public class PercentDynamicValue : DynamicValue
{
    public DynamicValue from;
    public DynamicValue percentage;
    protected override int ComputeValue(Context context)
    {
        return Mathf.RoundToInt(from.Compute(context) * percentage.Compute(context) / 100.0f);
    }
}