using UnityEngine;

[CreateAssetMenu(fileName = "ContextValue", menuName = "DynamicValue/ContextValue")]
public class ContextValueDynamicValue : DynamicValue
{
    public override int ComputeValue(Context context)
    {
        return context.value;
    }
}