using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/DealDamage")]
public class DealDamageEffect : Effect
{
    public float damage;
    public float damagePerLevel;
    public int duration;
    public bool modified;

    public override void Apply(Entity source, Entity target, int level)
    {
        float ratio = modified ? source.damageModifier/100 : 1;

        target.SendMessage("DealDamage", (damage+damagePerLevel*level)*ratio,SendMessageOptions.DontRequireReceiver);
        if (duration != 0)
        {
            GameObject newPassive = Instantiate(target.passivePrefab, target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            target.passiveObjects.Add(passiveComponent);
            passiveComponent.level = level;
            passiveComponent.holder = target;
            passiveComponent.definition = ScriptableObject.CreateInstance<PassiveDefinition>();
            passiveComponent.definition.trigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.triggerCount = 1;
            passiveComponent.definition.endTrigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.endTriggerCount = duration;
            passiveComponent.definition.targets = ScriptableObject.CreateInstance<PassiveHolderTargetSelector>();
            passiveComponent.definition.conditions = new List<Condition>();
            passiveComponent.definition.effects = new List<Effect>()
            {
                ScriptableObject.CreateInstance<DealDamageEffect>()
            };
            (passiveComponent.definition.effects[0] as DealDamageEffect).damage = damage*ratio;
            (passiveComponent.definition.effects[0] as DealDamageEffect).damagePerLevel = damagePerLevel*ratio;
            (passiveComponent.definition.effects[0] as DealDamageEffect).duration = 0;

        }
    }
}
