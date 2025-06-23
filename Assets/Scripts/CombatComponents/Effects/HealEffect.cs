using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Effects/Heal")]
public class HealEffect : Effect
{
    public DynamicValue value;
    public int duration;
    public bool fromAbility = true;
    public bool canCrit = true;
    public override void Apply(Context context)
    {
        float critModifier = canCrit && context.isCritical && context.source is Hero heroSource
            ? heroSource.GetCarac(Carac.critPower)
            : 100;        
        int healValue = Mathf.RoundToInt(value.computeValue(context) * critModifier/100f);
        context.value = healValue;
        context.target.Heal(context);

        if (context.source is Hero hero)
        {
            hero.threat += healValue * hero.definition.ThreatRatio;
            FightManager.instance.UpdateMostThreatHero(hero);
        }

        if (duration != 0)
        {
            Passive newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
            context.target.passives.Add(newPassive);
            newPassive.level = context.level;
            newPassive.holder = context.target;
            newPassive.definition = CreateInstance<PassiveDefinition>();
            newPassive.definition.trigger = Trigger.EveryPersonalTick;
            newPassive.definition.triggerCount = 10;
            newPassive.definition.endTrigger = Trigger.EveryPersonalTick;
            newPassive.definition.endTriggerCount = duration;
            newPassive.definition.targets = CreateInstance<PassiveHolderTargetSelector>();
            newPassive.definition.conditions = new List<Condition>();
            newPassive.definition.endTriggerConditions = new List<Condition>();
            newPassive.definition.model = FXManager.instance.healingParticlesPrefab;
            newPassive.definition.effects = new List<Effect>
            {
                CreateInstance<HealEffect>()
            };
            ((HealEffect)newPassive.definition.effects[0]).value = value;
            ((HealEffect)newPassive.definition.effects[0]).duration = 0;
        }
    }
}