using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Effects/Heal")]
public class HealEffect : Effect
{
    public DynamicValue value;
    public int duration;
    public bool modified = true;
    public bool canCrit = true;
    public override void Apply(Context context)
    {
        float critModifier = canCrit && context.isCritical && context.source is Hero heroSource ? heroSource.critPower : 100;
        float ratio = modified ? (100+context.source.percentPower) / 100 : 1;
        int healValue = Mathf.RoundToInt(value.computeValue(context) * ratio * critModifier/100f);
        context.target.Heal(healValue,context.isCritical);

        if (context.source is Hero hero)
        {
            hero.threat += healValue * hero.definition.threatRatio;
            FightManager.instance.UpdateMostThreatHero(hero);
        }

        if (duration != 0)
        {
            GameObject newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            context.target.passiveObjects.Add(passiveComponent);
            passiveComponent.level = context.level;
            passiveComponent.holder = context.target;
            passiveComponent.definition = CreateInstance<PassiveDefinition>();
            passiveComponent.definition.trigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.triggerCount = 10;
            passiveComponent.definition.endTrigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.endTriggerCount = duration;
            passiveComponent.definition.targets = CreateInstance<PassiveHolderTargetSelector>();
            passiveComponent.definition.conditions = new List<Condition>();
            passiveComponent.definition.effects = new List<Effect>
            {
                CreateInstance<HealEffect>()
            };
            ((HealEffect)passiveComponent.definition.effects[0]).value = value;
            ((HealEffect)passiveComponent.definition.effects[0]).duration = 0;
        }
    }
}