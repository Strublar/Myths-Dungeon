using UnityEngine;

[CreateAssetMenu(fileName = "NegativeValue", menuName = "DynamicValue/Operation/Negative")]
public class NegativeDynamicValue : DynamicValue
{
    public DynamicValue value;

    public static NegativeDynamicValue CreateFrom(DynamicValue originValue)
    {
        var dynamicValue = CreateInstance<NegativeDynamicValue>();
        dynamicValue.value = originValue;
        return dynamicValue;
    }

    protected override int ComputeValue(Context context)
    {
        return -1 * value.Compute(context);
    }
}