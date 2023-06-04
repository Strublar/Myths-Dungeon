using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewCondition", menuName = "Boss/BossSpell")]
public abstract class Condition : ScriptableObject
{
    public abstract bool ShouldTrigger(Context context);
}
