using UnityEngine;

[CreateAssetMenu(fileName = "SpecificPassiveGainedCondition", menuName = "Conditions/SpecificPassiveGained")]
public class SpecificPassiveGainedCondition : Condition
{
    public PassiveDefinition definition;
    public override bool ShouldTrigger(Context context)
    {
        return context.passiveGained == definition;
    }
}