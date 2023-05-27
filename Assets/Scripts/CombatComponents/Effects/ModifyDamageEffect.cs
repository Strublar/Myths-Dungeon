using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ModifyDamage")]
public class ModifyDamageEffect : Effect
{
    public float damage;
    public float damagePerLevel;
    public int duration;
    public override void Apply(Entity source, Entity target, int level)
    {
        
        target.damageModifier += damage + damagePerLevel * level;
        if (duration != 0)
        {
            GameObject newPassive = Instantiate(target.passivePrefab, target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            target.passiveObjects.Add(passiveComponent);
            passiveComponent.level = level;
            passiveComponent.holder = target;
            passiveComponent.definition = ScriptableObject.CreateInstance<PassiveDefinition>();
            passiveComponent.definition.trigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.triggerCount = duration;
            passiveComponent.definition.endTrigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.endTriggerCount = duration;
            passiveComponent.definition.targets = ScriptableObject.CreateInstance<PassiveHolderTargetSelector>();
            passiveComponent.definition.conditions = new List<Condition>();
            passiveComponent.definition.effects = new List<Effect>()
            {
                ScriptableObject.CreateInstance<ModifyDamageEffect>()
            };
            (passiveComponent.definition.effects[0] as ModifyDamageEffect).damage = -damage;
            (passiveComponent.definition.effects[0] as ModifyDamageEffect).damagePerLevel = -damagePerLevel;
            (passiveComponent.definition.effects[0] as ModifyDamageEffect).duration = 0;

        }
    }
}
