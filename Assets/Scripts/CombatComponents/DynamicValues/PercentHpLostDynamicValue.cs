using UnityEngine;

[CreateAssetMenu(fileName = "NewDynamicValue", menuName = "DynamicValue/Specific/PercentHpLost")]
public class PercentHpLostDynamicValue : DynamicValue
{
    public override int ComputeValue(Context context)
    {
        return context.percentHpLost;
    }
}