using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/HighestHealthHero")]
public class HighestHealthHeroTargetSelector : TargetSelector
{
    
    public override List<Entity> GetTargets(Context context)
    {
        List<Entity> newTargets = new List<Entity>();
        var aliveHeroes = (from hero in RunManager.instance.heroes where hero.isAlive select hero).ToList();
        int highestHp = aliveHeroes.Max(r => r.GetCarac(Carac.currentHp));
        var lowestHealth = from hero in aliveHeroes where hero.GetCarac(Carac.currentHp) == highestHp
                           select hero; 
        newTargets.Add(lowestHealth.First());
        return newTargets;
    }
}
