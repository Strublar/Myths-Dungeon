using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemRarityBased", menuName = "DynamicValue/ItemRarityBased")]
public class ItemRarityBasedValue : DynamicValue
{
    public DynamicValue baseValue;
    public DynamicValue valuePerLevel;
    public override int ComputeValue(Context context)
    {

        //TODO
        var value = baseValue.ComputeValue(context);
        /*if (context.source is Hero hero)
        {
            value += hero.item.qualityLevel * valuePerLevel.computeValue(context);
        }
        */
        

        return value;
    }
}