using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive")]
public class PassiveDefinition : ScriptableObject
{
    public string description;
    public Trigger trigger;
    public int triggerCount;
    public Trigger endTrigger;
    public int endTriggerCount;
    public TargetSelector targets;
    public List<Condition> conditions;
    public List<Effect> effects;
    [Header("Serialization")]
    public List<DynamicValue> values;

    public static PassiveDefinition BuildResourcePassive(Hero hero)
    {
        var passiveDef = CreateInstance<PassiveDefinition>();
        passiveDef.trigger = Trigger.EveryPersonalTick;
        passiveDef.triggerCount = hero.definition.resourceRegenerationTickDelay;
        passiveDef.endTrigger = Trigger.Never;
        passiveDef.endTriggerCount = 0;
        passiveDef.targets = CreateInstance<PassiveHolderTargetSelector>();
        passiveDef.conditions = new List<Condition>();
        passiveDef.values = new List<DynamicValue>();
        var dynamicValue = CreateInstance<ConstantDynamicValue>();
        dynamicValue.value = hero.definition.resourceRegeneration;
        var passiveEffect = CreateInstance<ModifyResourcesEffect>();
        passiveEffect.value = dynamicValue;
        passiveDef.effects = new List<Effect>
        {
            passiveEffect
        };
        return passiveDef;
    }
    
}
