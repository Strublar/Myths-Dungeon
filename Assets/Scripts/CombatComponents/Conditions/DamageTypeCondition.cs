using UnityEngine;

[CreateAssetMenu(fileName = "NewCondition", menuName = "Conditions/DamageType")]
public class DamageTypeCondition : Condition
{
    public DamageType damageType = DamageType.None;
    public override bool ShouldTrigger(Context context)
    {
        return context.damageType == damageType;
    }
}