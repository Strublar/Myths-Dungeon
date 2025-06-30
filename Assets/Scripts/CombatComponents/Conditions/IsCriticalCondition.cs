using System;
using UnityEngine;

[CreateAssetMenu(fileName = "isCritical", menuName = "Conditions/IsCritical")]
public class IsCriticalCondition : Condition
{

    public override bool ShouldTrigger(Context context)
    {
        return context.isCritical;
    }
}