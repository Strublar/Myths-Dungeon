using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldEffect", menuName = "Effects/Shield")]
public class ShieldEffect : Effect
{
    public DynamicValue value;
    public DynamicValue duration;
    public bool canCrit = true;
    public override void Apply(Context context)
    {
        float critModifier = canCrit && context.isCritical && context.source is Hero heroSource
            ? heroSource.GetCarac(Carac.critPower)
            : 100;        
        int shieldValue = Mathf.RoundToInt(value.computeValue(context) * critModifier/100f);
        
        context.target.AddShield(shieldValue,duration.computeValue(context));

        if (context.source is Hero hero)
        {
            hero.threat += shieldValue * hero.definition.ThreatRatio;
            FightManager.instance.UpdateMostThreatHero(hero);
        }
    }
}