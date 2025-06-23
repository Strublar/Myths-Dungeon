using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "OtherLowestHealthHero" , menuName = "TargetSelector/OtherLowestHealthHero")]
public class OtherLowestHealthHeroTargetSelector : TargetSelector
{
    public override List<Entity> GetTargets(Context context)
    {
        List<Entity> newTargets = new List<Entity>();
        var aliveHeroes = (from hero in RunManager.instance.heroes where hero.isAlive && hero != context.passiveHolder select hero).ToList();
        int lowestHp = aliveHeroes.Max(r => r.GetCarac(Carac.currentHp));
        var lowestHealth = from hero in aliveHeroes
            where hero.GetCarac(Carac.currentHp) == lowestHp
            select hero;
        newTargets.Add(lowestHealth.First());
        return newTargets;
    }
}