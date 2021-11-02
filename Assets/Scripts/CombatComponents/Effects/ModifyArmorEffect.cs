using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ModifyArmorEffect")]
public class ModifyArmorEffect : Effect
{
    public float armor;
    public float armorPerLevel;
    public int duration;
    public override void Apply(Entity source, Entity target, int level)
    {
        float armorModif = armor + armorPerLevel * level;
        target.armor += armorModif;
        if (Mathf.RoundToInt(armorModif) != 0)
        {
            GameObject bubble = Instantiate(target.armorBubble, target.transform);
            bubble.transform.localPosition = new Vector3(Random.Range(-.6f, .6f), -1, 0);
            bubble.GetComponent<DamageBubbleBehaviour>().text.text = (armorModif>0 ?"+":"") + Mathf.RoundToInt(armorModif);
        }

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
                ScriptableObject.CreateInstance<ModifyArmorEffect>()
            };
            (passiveComponent.definition.effects[0] as ModifyArmorEffect).armor = -armor;
            (passiveComponent.definition.effects[0] as ModifyArmorEffect).armorPerLevel = -armorPerLevel;
            (passiveComponent.definition.effects[0] as ModifyArmorEffect).duration = 0;

        }
    }
}
