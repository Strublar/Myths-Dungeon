using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/HighestHealthHero")]
public class HighestHealthHeroTargetSelector : TargetSelector
{
    
    public override List<Entity> GetTargets(Context context)
    {
        double max = Double.MinValue;
        Hero target = null;
        foreach (var hero in RunManager.instance.heroes)
        {
            if (!hero.isAlive) continue;
            double hp = (double)hero.GetCarac(Carac.CurrentHp) / hero.GetCarac(Carac.MaxHp);
            if ( hp > max)
            {
                target = hero;
                max = hp;
            }
        }

        if (target == null) return new List<Entity>();
        
        
        return new List<Entity>(){target};
    }
}
