using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/Heal")]
public class HealEffect : Effect
{
    public float value;
    public float valuePerLevel;
    public int duration;
    public bool modified = false;
    public override void Apply(Entity source, Entity target, int level)
    {
        float ratio = modified ? source.damageModifier/100 :  1;
        target.SendMessage("Heal", (value+valuePerLevel*level)*ratio, SendMessageOptions.DontRequireReceiver);

        if (duration != 0)
        {
            GameObject newPassive = Instantiate(target.passivePrefab, target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            target.passiveObjects.Add(passiveComponent);
            passiveComponent.level = level;
            passiveComponent.holder = target;
            passiveComponent.definition = ScriptableObject.CreateInstance<PassiveDefinition>();
            passiveComponent.definition.trigger = Trigger.EveryPersonalSecond;
            passiveComponent.definition.triggerCount = 1;
            passiveComponent.definition.endTrigger = Trigger.EveryPersonalSecond;
            passiveComponent.definition.endTriggerCount = duration;
            passiveComponent.definition.targets = ScriptableObject.CreateInstance<PassiveHolderTargetSelector>();
            passiveComponent.definition.conditions = new List<Condition>();
            passiveComponent.definition.effects = new List<Effect>()
            {
                ScriptableObject.CreateInstance<HealEffect>()
            };
            (passiveComponent.definition.effects[0] as HealEffect).value = value* ratio;
            (passiveComponent.definition.effects[0] as HealEffect).valuePerLevel = valuePerLevel* ratio;
            (passiveComponent.definition.effects[0] as HealEffect).duration = 0;

        }
    }
}
