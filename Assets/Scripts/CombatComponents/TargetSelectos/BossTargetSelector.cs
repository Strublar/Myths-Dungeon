using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/Boss")]
public class BossTargetSelector : TargetSelector
{
    
    public override List<Entity> GetTargets(Context context)
    {

        return new List<Entity>() {GameManager.gm.boss };
    }
}
