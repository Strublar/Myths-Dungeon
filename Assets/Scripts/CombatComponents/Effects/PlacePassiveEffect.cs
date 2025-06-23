using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlacePassiveEffect", menuName = "Effects/PlacePassive")]
public class PlacePassiveEffect : Effect
{
    public PassiveDefinition passive;

    public override void Apply(Context context)
    {
        context.passiveGained = passive;
        Passive newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
        newPassive.holder = context.target;
        newPassive.definition = passive;
        newPassive.level = context.level;
        context.target.passives.Add(newPassive);

        TriggerManager.triggerMap[Trigger.OnPassiveGained].Invoke(context);
    }
}
 