using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemQualityBased", menuName = "DynamicValue/ItemQualityBased")]
public class ItemQualityBasedValue : DynamicValue
{
    public DynamicValue baseValue;
    public DynamicValue valuePerLevel;
    public override int computeValue(Context context)
    {

        var value = baseValue.computeValue(context);
        if (context.source is Hero hero)
        {
            value += hero.item.qualityLevel * valuePerLevel.computeValue(context);
        }

        return value;
    }
}