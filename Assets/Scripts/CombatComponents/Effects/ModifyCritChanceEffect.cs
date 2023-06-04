using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ModifyCritChance")]
public class ModifyCritChanceEffect : Effect
{
    public DynamicValue value;
    public int duration;

    public override void Apply(Context context)
    {
        if (context.target is Hero hero)
        {
            hero.critChance += value.computeValue(context);
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
                    CreateInstance<ModifyCritChanceEffect>()
                };
                ((ModifyHasteEffect)passiveComponent.definition.effects[0]).value =
                    NegativeDynamicValue.CreateFrom(value);
                ((ModifyHasteEffect)passiveComponent.definition.effects[0]).duration = 0;
            }
        }
    }
}