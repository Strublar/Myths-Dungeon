using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetSelector", menuName = "TargetSelector/Source")]
public class SourceTargetSelector : TargetSelector
{
    
    public override List<Entity> GetTargets(Context context)
    {
        return new List<Entity>() {context.source };
    }
}
