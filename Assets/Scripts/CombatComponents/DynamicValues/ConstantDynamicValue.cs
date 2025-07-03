using UnityEngine;

[CreateAssetMenu(fileName = "Value", menuName = "DynamicValue/Constant")]
public class ConstantDynamicValue : DynamicValue
{

    public int value;
    public int valuePerLevel;
    
    public static ConstantDynamicValue Create(int constValue)
    {
        var dynamicValue = CreateInstance<ConstantDynamicValue>();
        dynamicValue.value = constValue;
        return dynamicValue;
    }
    public override int ComputeValue(Context context)
    {
        return value + valuePerLevel * (context.level - 1);
    }
}