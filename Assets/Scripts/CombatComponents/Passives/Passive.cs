using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour
{
    public int level;
    public Entity holder;
    public PassiveDefinition definition;
    public int triggerCount = 0;
    public int endTriggerCount = 0;

    public float clock = 0.05f;
    public float clockEnd = 0.05f;

    public void Start()
    {
        TriggerManager.triggerMap[definition.trigger].AddListener(Execute);
        TriggerManager.triggerMap[definition.endTrigger].AddListener(OnEndTrigger);
    }


    public void Update()
    {
        if (RunManager.instance.fightStarted)
        {
            if (definition.trigger == Trigger.EveryPersonalTick)
            {
                clock -= Time.deltaTime;
                while (clock <= 0)
                {
                    Context context = new Context
                    {
                        passiveHolder = holder,
                        level = level
                    };
                    Execute(context);
                    clock += 0.1f;
                }
            }

            if (definition.endTrigger == Trigger.EveryPersonalTick)
            {
                clockEnd -= Time.deltaTime;
                while (clockEnd <= 0)
                {
                    Context context = new Context
                    {
                        passiveHolder = holder
                    };
                    OnEndTrigger(context);
                    clockEnd += 0.1f;
                }
            }
        }
    }

    public void Execute(Context context)
    {
        triggerCount++;
        if (triggerCount >= definition.triggerCount && definition.triggerCount != 0)
        {
            context.passiveHolder = holder;
            bool shouldTrigger = true;
            foreach (Condition condition in definition.conditions)
            {
                if (!condition.ShouldTrigger(context))
                {
                    shouldTrigger = false;
                    break;
                }
            }

            if (shouldTrigger)
            {
                context.source = holder;
                foreach (Effect effect in definition.effects)
                {
                    foreach (Entity target in definition.targets.GetTargets(context))
                    {
                        context.target = target;
                        effect.Apply(context);
                    }
                }
            }

            triggerCount = 0;
        }
    }

    public void OnEndTrigger(Context context)
    {
        endTriggerCount++;
        if (endTriggerCount >= definition.endTriggerCount && definition.endTriggerCount != 0)
        {
            Delete(context);
        }
    }

    public void Delete(Context context)
    {
        TriggerManager.triggerMap[definition.trigger].RemoveListener(Execute);
        TriggerManager.triggerMap[definition.endTrigger].RemoveListener(OnEndTrigger);
        Destroy(gameObject);

        if (context.passiveHolder != null)
            holder.passiveObjects.Remove(this);
    }
}