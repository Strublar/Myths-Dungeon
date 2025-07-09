using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddValue", menuName = "DynamicValue/Operation/Add")]
public class AddDynamicValue : DynamicValue
{
    public List<DynamicValue> values;
    protected override int ComputeValue(Context context)
    {
        var finalValue = 0;
        foreach (var dynamicValue in values)
        {
            finalValue += dynamicValue.Compute(context);
        }

        return finalValue;
    }
    
}