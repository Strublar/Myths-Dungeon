using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ModifyHaste")]
public class ModifyHasteEffect : Effect
{
    public float haste;
    public float hastePerLevel;
    public int duration;
    public override void Apply(Entity source, Entity target, int level)
    {
        
        target.haste += haste + hastePerLevel * level;
        if (duration != 0)
        {
            GameObject newPassive = Instantiate(target.passivePrefab, target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            target.passiveObjects.Add(passiveComponent);
            passiveComponent.level = level;
            passiveComponent.holder = target;
            passiveComponent.definition = ScriptableObject.CreateInstance<PassiveDefinition>();
            passiveComponent.definition.trigger = Trigger.EveryPersonalSecond;
            passiveComponent.definition.triggerCount = duration;
            passiveComponent.definition.endTrigger = Trigger.EveryPersonalSecond;
            passiveComponent.definition.endTriggerCount = duration;
            passiveComponent.definition.targets = ScriptableObject.CreateInstance<PassiveHolderTargetSelector>();
            passiveComponent.definition.conditions = new List<Condition>();
            passiveComponent.definition.effects = new List<Effect>()
            {
                ScriptableObject.CreateInstance<ModifyHasteEffect>()
            };
            (passiveComponent.definition.effects[0] as ModifyHasteEffect).haste = -haste;
            (passiveComponent.definition.effects[0] as ModifyHasteEffect).hastePerLevel = -hastePerLevel;
            (passiveComponent.definition.effects[0] as ModifyHasteEffect).duration = 0;

        }
    }
}
