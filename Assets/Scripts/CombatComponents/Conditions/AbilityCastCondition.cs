using UnityEngine;

[CreateAssetMenu(fileName = "AbilityCastCondition", menuName = "Conditions/AbilityCast")]
public class AbilityCastCondition : Condition
{
    public AbilityDefinition Definition;

    public override bool ShouldTrigger(Context context)
    {
        return context.abilityCast == Definition;
    }
}