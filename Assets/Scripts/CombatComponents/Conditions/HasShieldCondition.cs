using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HasShieldCondition", menuName = "Conditions/HasShield")]
public class HasShieldCondition : Condition
{
    public TargetSelector selector;
    public override bool ShouldTrigger(Context context)
    {
        var targets = selector.GetTargets(context);
        return targets.All(t => t.shieldValues.Count > 0) ;
    }
}