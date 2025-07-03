using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PercentDynamicValue", menuName = "DynamicValue/PercentDynamicValue")]
public class PercentDynamicValue : DynamicValue
{
    public DynamicValue from;
    public DynamicValue percentage;
    public override int ComputeValue(Context context)
    {
        return Mathf.RoundToInt(from.ComputeValue(context) * percentage.ComputeValue(context) / 100.0f);
    }
}