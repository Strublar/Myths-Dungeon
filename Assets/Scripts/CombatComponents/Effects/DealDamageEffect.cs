using System.Collections;
using System.Collections.Generic;
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
            ? heroSource.GetCarac(Carac.critPower)
            : 100;
        float abilityBuff = fromAbility ? (float)(100 + context.source.GetCarac(Carac.abilityPower)) / 100 : 1;
        int damageValue = Mathf.RoundToInt(value.computeValue(context) * abilityBuff * critModifier / 100f);
        context.value = damageValue;
        context.damageType = damageType;
        context.target.DealDamage(context);
        if (context.source is Hero hero)
        {
            hero.threat += damageValue * hero.definition.threatRatio;
            FightManager.instance.UpdateMostThreatHero(hero);
        }

        if (duration != 0)
        {
            GameObject newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            context.target.passiveObjects.Add(passiveComponent);
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
                CreateInstance<DealDamageEffect>()
            };
            ((DealDamageEffect)passiveComponent.definition.effects[0]).value = value;
            ((DealDamageEffect)passiveComponent.definition.effects[0]).duration = 0;
            ((DealDamageEffect)passiveComponent.definition.effects[0]).damageType = damageType;
        }
    }
}