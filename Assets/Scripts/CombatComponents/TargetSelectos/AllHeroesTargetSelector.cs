using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/AllHeroes")]
public class AllHeroesTargetSelector : TargetSelector
{
    
    public override List<Entity> GetTargets(Context context)
    {
        List<Entity> newTargets = new List<Entity>();
        foreach(Hero hero in GameManager.gm.heroes)
        {
            newTargets.Add(hero);
        }
        return newTargets;
    }
}
