using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PercentDynamicValue", menuName = "DynamicValue/PercentDynamicValue")]
public class PercentDynamicValue : DynamicValue
{
    public DynamicValue from;
    public DynamicValue percentage;
    public override int computeValue(Context context)
    {
        return Mathf.RoundToInt(from.computeValue(context) * percentage.computeValue(context) / 100.0f);
    }
}