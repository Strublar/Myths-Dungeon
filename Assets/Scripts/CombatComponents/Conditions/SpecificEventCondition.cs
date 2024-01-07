using UnityEngine;

[CreateAssetMenu(fileName = "SpecificEventCondition", menuName = "Conditions/SpecificEvent")]
public class SpecificEventCondition : Condition
{
    public SpecificEvent specificEvent;
    public override bool ShouldTrigger(Context context)
    {
        return context.specificEvent == specificEvent;
    }
}