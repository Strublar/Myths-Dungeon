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
        var aliveHeroes = from hero in GameManager.gm.heroes where hero.isAlive select hero;
        float test = aliveHeroes.Max(r => r.currentHp);
        var lowestHealth = from hero in aliveHeroes where hero.currentHp == aliveHeroes.Max(r => r.currentHp)
                           select hero; 
        newTargets.Add(lowestHealth.First());
        return newTargets;
    }
}
