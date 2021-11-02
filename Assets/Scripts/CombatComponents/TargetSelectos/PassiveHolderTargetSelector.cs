using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/PassiveHolder")]
public class PassiveHolderTargetSelector : TargetSelector
{
    
    public override List<Entity> GetTargets(Context context)
    {
        return new List<Entity>() {context.passiveHolder };
    }
}
