using System.Collections.Generic;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "CaracEffect", menuName = "Effects/ModifyCaracEffect")]
public class ModifyCaracEffect : Effect
{
    public Carac carac;
    public DynamicValue value;
    public int duration;
    public override void Apply(Context context)
    {
        int modif = value.computeValue(context);
        context.target.caracs[carac] += modif;

        if (duration != 0)
        {
            Passive newPassive = PassivePool.instance.GetObject(context.target.transform);
            context.target.passives.Add(newPassive);
            newPassive.holder = context.target;
            newPassive.definition = CreateInstance<PassiveDefinition>();
            newPassive.definition.trigger = Trigger.EveryPersonalTick;
            newPassive.definition.triggerCount = duration;
            newPassive.definition.endTrigger = Trigger.EveryPersonalTick;
            newPassive.definition.endTriggerCount = duration;
            newPassive.definition.targets = CreateInstance<PassiveHolderTargetSelector>();
            newPassive.definition.conditions = new List<Condition>();
            newPassive.definition.endTriggerConditions = new List<Condition>();
            newPassive.definition.applyOutOfFight = true;
            newPassive.definition.effects = new List<Effect>
            {
                CreateInstance<ModifyCaracEffect>()
            };
            ((ModifyCaracEffect)newPassive.definition.effects[0]).carac = carac;
            ((ModifyCaracEffect)newPassive.definition.effects[0]).value = ConstantDynamicValue.Create(-1*modif);
            ((ModifyCaracEffect)newPassive.definition.effects[0]).duration = 0;
            newPassive.Init();
        }
    }
}
