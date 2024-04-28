using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PowerDynamicValue", menuName = "DynamicValue/PowerDynamicValue")]
public class PowerDynamicValue : DynamicValue
{
    public override int computeValue(Context context)
    {
        if (context.source is Hero hero)
            return hero.power;

        return 0;
    }
}