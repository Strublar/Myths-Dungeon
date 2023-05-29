using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/RandomHero")]
public class RandomHeroTargetSelector : TargetSelector
{
    public int count;
    public override List<Entity> GetTargets(Context context)
    {
        List<Entity> newTargets = new List<Entity>();
        List<Hero> aliveHeroes = (from hero in RunManager.instance.heroes
                                 where hero.isAlive
                                 select hero).ToList();
        if (aliveHeroes.Count <= count)
        {
            foreach(Hero hero in aliveHeroes)
            {
                newTargets.Add(hero);
            }
            return newTargets;
        }
            

        while(newTargets.Count<count)
        {
            Entity randomHero = aliveHeroes[Random.Range(0, aliveHeroes.Count)];
            if(!newTargets.Contains(randomHero))
            {
                newTargets.Add(randomHero);
                
            }

        }

        return newTargets;
    }
}
