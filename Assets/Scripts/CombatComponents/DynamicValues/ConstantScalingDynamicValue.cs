using System;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Value", menuName = "DynamicValue/ConstantScaling")]
public class ConstantScalingDynamicValue : DynamicValue
{

    public int value;
    public Carac scalingCarac;
    public override int computeValue(Context context)
    {
        if (context.passiveHolder == null) return value;
        if (context.passiveHolder is Hero hero)
        {
            var baseCaracValue = hero.definition.caracs.Find(c => c.carac == scalingCarac);

            int offset = 0;
            switch (scalingCarac)
            {
                case Carac.Armor:
                    offset = 100;
                    break;
                case Carac.CritChance:
                    offset = 100;
                    break;
                case Carac.Mastery:
                    offset = 100;
                    break;
                default:
                    offset = 0;
                    break;
            }

            return value * (context.passiveHolder.GetCarac(scalingCarac) + offset) / (baseCaracValue.value+offset);
        } 
        return value * context.passiveHolder.GetCarac(scalingCarac)/100;
    }
}