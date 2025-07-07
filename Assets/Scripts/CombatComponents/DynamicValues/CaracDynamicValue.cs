using System;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CaracValue", menuName = "DynamicValue/CaracValue")]
public class CaracDynamicValue : DynamicValue
{
    public TargetSelector entities;
    [FormerlySerializedAs("stat")] public Carac carac;
    protected  override int ComputeValue(Context context)
    {
        var targets = entities.GetTargets(context);
        var value = 0;
        foreach (var target in targets)
        {
            value += target.GetCarac(carac);
        }
        return value;
    }
}