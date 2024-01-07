using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomEffect", menuName = "Effects/Random")]

public class RandomEffects : Effect
{
    public DynamicValue probability;
    public List<Effect> childEffects;
    public override void Apply(Context context)
    {
        if(Random.Range(0,100) <= probability.computeValue(context))
        {
            foreach (var effect in childEffects)
            {
                effect.Apply(context);
            }
        }

    }
}