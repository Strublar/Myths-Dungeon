using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroClassTargetSelector", menuName = "TargetSelector/HeroClass")]
public class HeroClassTargetSelector : TargetSelector
{
    public HeroClass heroClass;
    
    public override List<Entity> GetTargets(Context context)
    {
        return RunManager.instance.heroes.Where(hero => hero.definition.heroClass == heroClass).ToList<Entity>();
    }
}