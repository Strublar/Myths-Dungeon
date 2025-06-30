using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroTypeTargetSelector", menuName = "TargetSelector/HeroType")]
public class HeroTypeTargetSelector : TargetSelector
{
    public HeroType heroType;
    
    public override List<Entity> GetTargets(Context context)
    {
        return RunManager.instance.heroes.Where(hero => hero.definition.type == heroType).ToList<Entity>();
    }
}