using UnityEngine;

[CreateAssetMenu(fileName = "ContextValue", menuName = "DynamicValue/ContextValue")]
public class ContextValueDynamicValue : DynamicValue
{
    protected override int ComputeValue(Context context)
    {
        return context.value;
    }
}