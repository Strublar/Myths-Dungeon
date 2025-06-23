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
            Passive newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
            context.target.passives.Add(newPassive);
            newPassive.holder = context.target;
            newPassive.definition = CreateInstance<PassiveDefinition>();
            newPassive.definition.trigger = Trigger.EveryPersonalTick;
            newPassive.definition.triggerCount = duration;
            newPassive.definition.endTrigger = Trigger.EveryPersonalTick;
            newPassive.definition.endTriggerCount = duration;
            newPassive.definition.targets = CreateInstance<PassiveHolderTargetSelector>();
            newPassive.definition.conditions = new List<Condition>();
            newPassive.definition.endTriggerConditions = new List<Condition>();
            newPassive.definition.effects = new List<Effect>
            {
                CreateInstance<ModifyCaracEffect>()
            };
            ((ModifyCaracEffect)newPassive.definition.effects[0]).carac = carac;
            ((ModifyCaracEffect)newPassive.definition.effects[0]).value = NegativeDynamicValue.CreateFrom(value);
            ((ModifyCaracEffect)newPassive.definition.effects[0]).duration = 0;

        }
    }
}
