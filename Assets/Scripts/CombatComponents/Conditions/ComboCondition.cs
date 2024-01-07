using UnityEngine;

[CreateAssetMenu(fileName = "ComboCondition", menuName = "Conditions/Combo")]
public class ComboCondition : Condition
{
    public string lastAbilityName;
    public override bool ShouldTrigger(Context context)
    {
        return FightManager.instance.lastAbilityName.ToUpper().Contains(lastAbilityName.ToUpper());
    }
}