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
        var aliveHeroes = (from hero in RunManager.instance.heroes where hero.isAlive select hero).ToList();
        int lowestHp = aliveHeroes.Max(r =>  r.GetCarac(Carac.currentHp));
        var lowestHealth = from hero in aliveHeroes where hero.GetCarac(Carac.currentHp) == lowestHp
                           select hero; 
        newTargets.Add(lowestHealth.First());
        return newTargets;
    }
}
