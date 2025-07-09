using UnityEngine;

//[CreateAssetMenu(fileName = "NewCondition", menuName = "Boss/BossSpell")]

public abstract class DynamicValue : ScriptableObject
{
    public int Compute(Context context)
    {
        if (context.modifiedDynamicValues != null &&
            context.modifiedDynamicValues.ContainsKey(this))
        {
            var modification = 0;
            context.modifiedDynamicValues[this].ForEach(d => modification += d.ComputeValue(context));
            return Mathf.RoundToInt(ComputeValue(context) *
                (100f + modification) / 100);
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