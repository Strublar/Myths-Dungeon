using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : ScriptableObject
{
    
    public virtual List<Entity> GetTargets(Context context)
    {
        return null;
    }
}
