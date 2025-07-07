using UnityEngine;

[CreateAssetMenu(fileName = "NewDynamicValue", menuName = "DynamicValue/Specific/PercentHpLost")]
public class PercentHpLostDynamicValue : DynamicValue
{
    protected override int ComputeValue(Context context)
    {
        return context.percentHpLost;
    }
}