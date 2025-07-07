using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockAbility", menuName = "Effects/UnlockAbility")]

public class UnlockAbilityEffect : Effect
{
    public AbilityDefinition abilityToUnlock;
    public GameObject model;
    public GameObject orbitalObjectModel;
    
    public override void Apply(Context context)
    {
        PassiveDefinition passiveDef = CreateInstance<PassiveDefinition>();
        passiveDef.endTrigger = Trigger.OnUseAbility;
        passiveDef.endTriggerCount = 1;
        passiveDef.endTriggerConditions = new List<Condition>()
        {
            FromThisReplacementAbilityCondition.instance,
            HolderIsSourceCondition.instance
        };
        passiveDef.model = model;
        passiveDef.orbitalObjectModel = orbitalObjectModel;
        passiveDef.replacementAbility = abilityToUnlock;
        
        context.passiveGained = passiveDef;
        Passive newPassive = PassivePool.instance.GetObject(context.target.transform);
        newPassive.holder = context.target;
        newPassive.definition = passiveDef;
        newPassive.level = context.level;
        context.target.passives.Add(newPassive);
        newPassive.Init();

        TriggerManager.triggerMap[Trigger.OnPassiveGained].Invoke(context);
    }
}