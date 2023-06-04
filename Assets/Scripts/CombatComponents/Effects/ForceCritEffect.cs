using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ForceCrit")]

public class ForceCritEffect : Effect
{
    public List<Effect> childEffects;
    public override void Apply(Context context)
    {
        if (!context.isCritical)
        {
            context.isCritical = true;
            //TODO Crit event
        }

        foreach (var effect in childEffects)
        {
            effect.Apply(context);
        }
    }
}