using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewCondition", menuName = "Boss/BossSpell")]
public class Condition : ScriptableObject
{
    public virtual bool ShouldTrigger(Context context)
    {
        return true;
    }
}
