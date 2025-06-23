using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "WithTargetEffect", menuName = "Effects/WithTarget")]

public class WithTargetEffect : Effect
{
    public TargetSelector target;
    public List<Effect> childEffects;
    public override void Apply(Context context)
    {
        var targets = target.GetTargets(context);
        foreach (var target in targets)
        {
            context.target = target;
            foreach (var effect in childEffects)
            {
                effect.Apply(context);
            }
        }
    }
}