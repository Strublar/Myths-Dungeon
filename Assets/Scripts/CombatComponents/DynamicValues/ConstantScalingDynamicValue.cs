using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Value", menuName = "DynamicValue/ConstantScaling")]
public class ConstantScalingDynamicValue : DynamicValue
{

    public int value;
    public Carac scalingCarac;
    public int valuePerLevel;
    public override int computeValue(Context context)
    {
        return (value + valuePerLevel * (context.level - 1)) * context.passiveHolder.GetCarac(scalingCarac)/100;
    }
}