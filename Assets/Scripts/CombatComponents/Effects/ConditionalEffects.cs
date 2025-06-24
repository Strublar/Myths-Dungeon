using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConditionalEffect", menuName = "Effects/Conditional")]

public class ConditionalEffects : Effect
{
    public List<Condition> conditions;
    public List<Effect> effectsIfTrue;
    public List<Effect> effectsIfFalse;
    
    public override void Apply(Context context)
    {
        bool conditionsValid = true;
        foreach (var condition in conditions)
        {
            conditionsValid &= condition.ShouldTrigger(context);
        }

        if (conditionsValid)
        {
            foreach (var effect in effectsIfTrue)
            {
                effect.Apply(context);
            }
        }
        else
        {
            foreach (var effect in effectsIfFalse)
            {
                effect.Apply(context);
            }
        }
    }
}