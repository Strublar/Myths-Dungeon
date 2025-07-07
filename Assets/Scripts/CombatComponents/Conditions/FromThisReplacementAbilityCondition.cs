using UnityEngine;

[CreateAssetMenu(fileName = "FromThisReplacementAbilityCondition", menuName = "Conditions/FromThisReplacementAbility")]
public class FromThisReplacementAbilityCondition : Condition
{
    public static FromThisReplacementAbilityCondition instance = CreateInstance<FromThisReplacementAbilityCondition>();
    public override bool ShouldTrigger(Context context)
    {
        return context.underlyingPassive == context.replacementAbilityPassive;
    }
}