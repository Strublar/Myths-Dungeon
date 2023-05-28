using UnityEngine;

//[CreateAssetMenu(fileName = "NewCondition", menuName = "Boss/BossSpell")]

public abstract class DynamicValue : ScriptableObject
{
    public abstract int computeValue(Context context);
}
