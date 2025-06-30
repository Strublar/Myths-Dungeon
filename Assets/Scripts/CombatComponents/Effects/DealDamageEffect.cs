using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public enum DamageType
{
    None,
    Bleed
}

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Effects/DealDamage")]
public class DealDamageEffect : Effect
{
    public DynamicValue value;
    public int duration;
    public bool fromAbility = false;
    public bool canCrit = true;
    public DamageType damageType = DamageType.None;

    public override void Apply(Context context)
    {
        float critModifier = canCrit && context.isCritical && context.source is Hero heroSource
            ? heroSource.GetCarac(Carac.CritPower)
            : 100;
        int damageValue = Mathf.RoundToInt(value.computeValue(context) *  critModifier / 100f);
        context.value = damageValue;
        context.damageType = damageType;
        context.target.DealDamage(context);
        if (context.source is Hero hero)
        {
            hero.threat += damageValue * hero.definition.ThreatRatio;
            FightManager.instance.UpdateMostThreatHero(hero);
        }

        if (duration != 0)
        {
            Passive newPassive = PassivePool.instance.GetObject(context.target.transform);
            context.target.passives.Add(newPassive);
            newPassive.holder = context.target;
            newPassive.definition = CreateInstance<PassiveDefinition>();
            newPassive.definition.trigger = Trigger.EveryPersonalTick;
            newPassive.definition.triggerCount = 10;
            newPassive.definition.endTrigger = Trigger.EveryPersonalTick;
            newPassive.definition.endTriggerCount = duration;
            newPassive.definition.targets = CreateInstance<PassiveHolderTargetSelector>();
            newPassive.definition.conditions = new List<Condition>();
            newPassive.definition.endTriggerConditions = new List<Condition>();
            newPassive.definition.effects = new List<Effect>
            {
                CreateInstance<DealDamageEffect>()
            };
            ((DealDamageEffect)newPassive.definition.effects[0]).value = value;
            ((DealDamageEffect)newPassive.definition.effects[0]).duration = 0;
            ((DealDamageEffect)newPassive.definition.effects[0]).damageType = damageType;
            newPassive.Init();
        }
    }
}