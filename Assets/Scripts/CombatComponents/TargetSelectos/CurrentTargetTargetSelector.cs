using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentTargetTargetSelector", menuName = "TargetSelector/CurrentTarget")]
public class CurrentTargetTargetSelector : TargetSelector
{
    
    public override List<Entity> GetTargets(Context context)
    {
        var targets = new List<Entity>();
        if(context.source is Hero hero)
            targets.Add(hero.currentTarget);
        return targets;
    }
}