using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/MostThreatHero")]
public class MostThreatHeroTargetSelector : TargetSelector
{
    
    public override List<Entity> GetTargets(Context context)
    {
        return new List<Entity>() {GameManager.gm.mostThreatHero };
    }
}
