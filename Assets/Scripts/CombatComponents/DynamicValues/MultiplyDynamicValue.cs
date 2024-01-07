using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiplyValue", menuName = "DynamicValue/Operation/Multiply")]
public class MultiplyDynamicValue : DynamicValue
{
    public List<DynamicValue> values;
    public override int computeValue(Context context)
    {
        var finalValue = 1;
        foreach (var dynamicValue in values)
        {
            finalValue *= dynamicValue.computeValue(context);
        }

        return finalValue;
    }
}