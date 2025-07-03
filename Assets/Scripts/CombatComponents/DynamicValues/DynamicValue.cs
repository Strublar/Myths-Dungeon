using UnityEngine;

//[CreateAssetMenu(fileName = "NewCondition", menuName = "Boss/BossSpell")]

public abstract class DynamicValue : ScriptableObject
{
    public abstract int ComputeValue(Context context);

    public virtual string ComputeString(Context context)
    {
        return ComputeValue(context).ToString();
    } 
}
