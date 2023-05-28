using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCondition", menuName = "Conditions/HolderIsSource")]
public class HolderIsSourceCondition : Condition
{
    public override bool ShouldTrigger(Context context)
    {
        Debug.Log("Holder : "+ (context.passiveHolder as Hero).definition.heroName);
        Debug.Log("source : "+ (context.source as Hero).definition.heroName);
        return context.passiveHolder == context.source ;
    }
}
