using UnityEngine;

[CreateAssetMenu(fileName = "NewCondition", menuName = "Conditions/HolderIsTarget")]
public class HolderIsTargetCondition : Condition
{
    public override bool ShouldTrigger(Context context)
    {
        return context.passiveHolder == context.target ;
    }
}