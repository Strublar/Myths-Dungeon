using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Trigger
{
    Never,
    OnPull,
    OnAttack,
    OnUseAbility,
    OnHeal,
    EveryTick,
    EveryPersonalTick,
    FightEnd,
    OnDamageReceived,
    OnSpecificEvent,
    OnPassiveGained,
    OnCrit
}

public struct Context
{
    public Entity source;
    public Entity target;
    public Entity passiveHolder;
    public int level;
    public int value;
    public int percentHpLost;
    public bool isCritical;
    public DamageType damageType;
    public SpecificEvent specificEvent;
    public AbilityDefinition abilityCast;
    public Passive underlyingPassive;
    public PassiveDefinition passiveGained;
}

public class TriggerManager : MonoBehaviour
{
    public static TriggerManager instance;
    public static UnityEvent<Context> Never = new UnityEvent<Context>();
    public static UnityEvent<Context> OnPull = new UnityEvent<Context>();
    public static UnityEvent<Context> OnAttack = new UnityEvent<Context>();
    public static UnityEvent<Context> OnUseAbility = new UnityEvent<Context>();
    public static UnityEvent<Context> OnHealed = new UnityEvent<Context>();
    public static UnityEvent<Context> EverySecond = new UnityEvent<Context>();
    public static UnityEvent<Context> EveryPersonalSecond = new UnityEvent<Context>();
    public static UnityEvent<Context> FightEnd = new UnityEvent<Context>();
    public static UnityEvent<Context> OnDamageReceived = new UnityEvent<Context>();
    public static UnityEvent<Context> OnSpecificEvent = new UnityEvent<Context>();
    public static UnityEvent<Context> OnPassiveGained = new UnityEvent<Context>();
    public static UnityEvent<Context> OnCrit = new UnityEvent<Context>();

    public static Dictionary<Trigger, UnityEvent<Context>> triggerMap = new Dictionary<Trigger, UnityEvent<Context>>
    {
        { Trigger.Never,Never},
        { Trigger.OnPull,OnPull},
        { Trigger.OnAttack,OnAttack},
        { Trigger.OnUseAbility,OnUseAbility},
        { Trigger.OnHeal,OnHealed},
        { Trigger.EveryTick,EverySecond},
        { Trigger.EveryPersonalTick,EveryPersonalSecond},
        { Trigger.FightEnd,FightEnd},
        { Trigger.OnDamageReceived,OnDamageReceived},
        { Trigger.OnSpecificEvent,OnSpecificEvent},
        { Trigger.OnPassiveGained,OnPassiveGained},
        { Trigger.OnCrit,OnCrit},
    };

    private float timer1tick = 0.1f;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if(RunManager.instance.fightStarted)
        {
            timer1tick -= Time.deltaTime;
            while(timer1tick<0)
            {
                timer1tick += 0.1f;
                triggerMap[Trigger.EveryTick].Invoke(new Context());
            }
        }
    }

}


