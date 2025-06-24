using System;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "OtherLowestHealthHero" , menuName = "TargetSelector/OtherLowestHealthHero")]
public class OtherLowestHealthHeroTargetSelector : TargetSelector
{
    public override List<Entity> GetTargets(Context context)
    {
        double min = Double.MaxValue;
        Hero target = null;
        foreach (var hero in RunManager.instance.heroes)
        {
            if (!hero.isAlive || hero == context.passiveHolder) continue;
            double hp = (double)hero.GetCarac(Carac.currentHp) / hero.GetCarac(Carac.maxHp);
            if ( hp < min)
            {
                target = hero;
                min = hp;
            }
        }

        if (target == null) return new List<Entity>();
        
        
        return new List<Entity>(){target};
    }
}