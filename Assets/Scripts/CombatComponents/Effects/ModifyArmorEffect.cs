using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ModifyArmorEffect")]
public class ModifyArmorEffect : Effect
{
    public DynamicValue value;
    public int duration;
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Apply(Context context)
    {
        int armorModif = value.computeValue(context);
        context.target.armor += armorModif;
        if (armorModif != 0)
        {
            GameObject bubble = Instantiate(context.target.armorBubble, context.target.transform);
            bubble.transform.localPosition = new Vector3(Random.Range(-.6f, .6f), -1, 0);
            bubble.GetComponent<DamageBubbleBehaviour>().text.text = (armorModif>0 ?"+":"") + Mathf.RoundToInt(armorModif);
        }

        if (duration != 0)
        {
            GameObject newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            context.target.passiveObjects.Add(passiveComponent);
            passiveComponent.level = context.level;
            passiveComponent.holder = context.target;
            passiveComponent.definition = CreateInstance<PassiveDefinition>();
            passiveComponent.definition.trigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.triggerCount = duration;
            passiveComponent.definition.endTrigger = Trigger.EveryPersonalTick;
            passiveComponent.definition.endTriggerCount = duration;
            passiveComponent.definition.targets = CreateInstance<PassiveHolderTargetSelector>();
            passiveComponent.definition.conditions = new List<Condition>();
            passiveComponent.definition.effects = new List<Effect>
            {
                CreateInstance<ModifyArmorEffect>()
            };
            ((ModifyArmorEffect)passiveComponent.definition.effects[0]).value = NegativeDynamicValue.CreateFrom(value);
            ((ModifyArmorEffect)passiveComponent.definition.effects[0]).duration = 0;

        }
    }
}
