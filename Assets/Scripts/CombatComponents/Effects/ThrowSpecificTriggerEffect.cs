using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ThrowSpecificTrigger")]
public class ThrowSpecificTriggerEffect : Effect
{
    public Trigger trigger;
    public override void Apply(Context context)
    {
        TriggerManager.triggerMap[trigger].Invoke(context);
    }
}