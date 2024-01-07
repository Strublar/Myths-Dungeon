using UnityEngine;

[CreateAssetMenu(fileName = "AbilityKeywordCondition", menuName = "Conditions/AbilityKeyword")]
public class AbilityKeywordCondition : Condition
{
    public string keyword = "";

    public override bool ShouldTrigger(Context context)
    {
        if (context.source is Hero hero)
        {
            return hero.ability.abilityName.ToUpper().Contains(keyword.ToUpper());
        }

        return false;

    }
}