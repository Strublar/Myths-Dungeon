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
        return value * context.passiveHolder.GetCarac(scalingCarac)/100;
    }
}