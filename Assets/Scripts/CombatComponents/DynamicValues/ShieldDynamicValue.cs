using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldValue", menuName = "DynamicValue/ShieldValue")]
public class ShieldDynamicValue : DynamicValue
{

    public TargetSelector targetSelector;
    protected override int ComputeValue(Context context)
    {
        return targetSelector.GetTargets(context).Sum(t => t.ComputeShieldValue());;
    }
}