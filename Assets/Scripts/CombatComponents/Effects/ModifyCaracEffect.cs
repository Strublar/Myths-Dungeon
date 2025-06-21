using System.Collections.Generic;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "CaracEffect", menuName = "Effects/ModifyCaracEffect")]
public class ModifyCaracEffect : Effect
{
    public Carac carac;
    public DynamicValue value;
    public int duration;
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Apply(Context context)
    {
        int armorModif = value.computeValue(context);
        context.target.caracs[carac] += armorModif;
        /*if (armorModif != 0)
        {
            BubbleBehaviour bubble = Instantiate(context.target.armorBubble, context.target.transform);
            bubble.transform.localPosition = new Vector3(Random.Range(-.6f, .6f), -1, 0);
            bubble.GetComponent<BubbleBehaviour>().text.text = (armorModif>0 ?"+":"") + Mathf.RoundToInt(armorModif);
        }*/

        if (duration != 0)
        {
            GameObject newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
            Passive passiveComponent = newPassive.GetComponent<Passive>();
            context.target.passives.Add(passiveComponent);
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
                CreateInstance<ModifyCaracEffect>()
            };
            ((ModifyCaracEffect)passiveComponent.definition.effects[0]).carac = carac;
            ((ModifyCaracEffect)passiveComponent.definition.effects[0]).value = NegativeDynamicValue.CreateFrom(value);
            ((ModifyCaracEffect)passiveComponent.definition.effects[0]).duration = 0;

        }
    }
}
