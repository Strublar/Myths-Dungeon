using UnityEngine;

[CreateAssetMenu(fileName = "NewDynamicValue", menuName = "DynamicValue/Operation/Negative")]
public class NegativeDynamicValue : DynamicValue
{
    public DynamicValue value;

    public static NegativeDynamicValue CreateFrom(DynamicValue originValue)
    {
        var dynamicValue = CreateInstance<NegativeDynamicValue>();
        dynamicValue.value = originValue;
        return dynamicValue;
    }

    public override int computeValue(Context context)
    {
        return -1 * value.computeValue(context);
    }
}