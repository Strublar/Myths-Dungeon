using UnityEngine;

[CreateAssetMenu(fileName = "NewCondition", menuName = "Conditions/Not")]
public class NotCondition : Condition
{
    public Condition condition;
    public override bool ShouldTrigger(Context context)
    {
        return !condition.ShouldTrigger(context);
    }
}