using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBuffEffect", menuName = "Effects/ModifyDamage")]
public class ModifyDamageEffect : Effect
{
    public DynamicValue value;
    public int duration;

    public override void Apply(Context context)
    {
        context.target.percentPower += value.computeValue(context);
        if (duration != 0)
        {
            GameObject newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            context.target.passiveObjects.Add(passiveComponent);
            passiveComponent.level = context.level;
            passiveComponent.holder = context.target;
            passiveComponent.definition = CreateInstance<PassiveDefinition>();
            passiveComponent.definition.trigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.triggerCount = duration;
            passiveComponent.definition.endTrigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.endTriggerCount = duration;
            passiveComponent.definition.targets = CreateInstance<PassiveHolderTargetSelector>();
            passiveComponent.definition.conditions = new List<Condition>();
            passiveComponent.definition.effects = new List<Effect>
            {
                CreateInstance<ModifyDamageEffect>()
            };
            ((ModifyDamageEffect)passiveComponent.definition.effects[0]).value = NegativeDynamicValue.CreateFrom(value);
            ((ModifyDamageEffect)passiveComponent.definition.effects[0]).duration = 0;
        }
    }
}