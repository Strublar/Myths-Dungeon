using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/LowestHealthHero")]
public class LowestHealthHeroTargetSelector : TargetSelector
{ 
    public override List<Entity> GetTargets(Context context)
    {
        List<Entity> newTargets = new List<Entity>();
        var aliveHeroes = from hero in RunManager.instance.heroes where hero.isAlive select hero;
        float test = aliveHeroes.Max(r => r.currentHp);
        var lowestHealth = from hero in aliveHeroes where hero.currentHp == aliveHeroes.Min(r => r.currentHp)
                           select hero; 
        newTargets.Add(lowestHealth.First());
        return newTargets;
    }
}
