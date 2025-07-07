using UnityEngine;

//[CreateAssetMenu(fileName = "NewCondition", menuName = "Boss/BossSpell")]

public abstract class DynamicValue : ScriptableObject
{
    public int Compute(Context context)
    {
        if (context.replacedDynamicValues != null &&
            context.replacedDynamicValues.ContainsKey(this))
        {
            return context.replacedDynamicValues[this].ComputeValue(context);
        }
        else
        {
            return ComputeValue(context);
        }
    }
    protected abstract int ComputeValue(Context context);

    public virtual string ComputeString(Context context)
    {
        return Compute(context).ToString();
    } 
}
