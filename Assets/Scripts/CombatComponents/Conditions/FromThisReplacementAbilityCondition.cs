using UnityEngine;

[CreateAssetMenu(fileName = "FromThisReplacementAbilityCondition", menuName = "Conditions/FromThisReplacementAbility")]
public class FromThisReplacementAbilityCondition : Condition
{
    public override bool ShouldTrigger(Context context)
    {
        return context.underlyingPassive == context.replacementAbilityPassive;
    }
}