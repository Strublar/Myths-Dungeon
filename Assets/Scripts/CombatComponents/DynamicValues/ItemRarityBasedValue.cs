using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemRarityBased", menuName = "DynamicValue/ItemRarityBased")]
public class ItemRarityBasedValue : DynamicValue
{
    public DynamicValue baseValue;
    public DynamicValue valuePerLevel;
    public override int computeValue(Context context)
    {

        //TODO
        var value = baseValue.computeValue(context);
        /*if (context.source is Hero hero)
        {
            value += hero.item.qualityLevel * valuePerLevel.computeValue(context);
        }
        */
        

        return value;
    }
}