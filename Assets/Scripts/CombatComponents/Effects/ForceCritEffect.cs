using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForceCritEffect", menuName = "Effects/ForceCrit")]

public class ForceCritEffect : Effect
{
    public List<Effect> childEffects;
    public override void Apply(Context context)
    {
        if (!context.isCritical)
        {
            context.isCritical = true;
            TriggerManager.triggerMap[Trigger.OnCrit].Invoke(context);
        }

        foreach (var effect in childEffects)
        {
            effect.Apply(context);
        }
    }
}