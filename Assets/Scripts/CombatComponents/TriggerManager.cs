using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Trigger
{
    Never,
    OnPull,
    OnAttack,
    OnHeal,
    EveryTick,
    EveryPersonalTick,
    FightEnd,
    OnDamageReceived,
}

public struct Context
{
    public Entity source;
    public Entity target;
    public Entity passiveHolder;
    public int level;
    public int value;
    public int percentHpLost;

}

public class TriggerManager : MonoBehaviour
{
    public static TriggerManager instance;
    public static UnityEvent<Context> Never = new UnityEvent<Context>();
    public static UnityEvent<Context> OnPull = new UnityEvent<Context>();
    public static UnityEvent<Context> OnAttack = new UnityEvent<Context>();
    public static UnityEvent<Context> OnHealed = new UnityEvent<Context>();
    public static UnityEvent<Context> EverySecond = new UnityEvent<Context>();
    public static UnityEvent<Context> EveryPersonalSecond = new UnityEvent<Context>();
    public static UnityEvent<Context> FightEnd = new UnityEvent<Context>();
    public static UnityEvent<Context> OnDamageReceived = new UnityEvent<Context>();

    public static Dictionary<Trigger, UnityEvent<Context>> triggerMap = new Dictionary<Trigger, UnityEvent<Context>>
    {
        { Trigger.Never,Never},
        { Trigger.OnPull,OnPull},
        { Trigger.OnAttack,OnAttack},
        { Trigger.OnHeal,OnHealed},
        { Trigger.EveryTick,EverySecond},
        { Trigger.EveryPersonalTick,EveryPersonalSecond},
        { Trigger.FightEnd,FightEnd},
        { Trigger.OnDamageReceived,OnDamageReceived},
    };

    private float timer1s = 0.1f;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if(GameManager.gm.fightStarted)
        {
            timer1s -= Time.deltaTime;
            while(timer1s<0)
            {
                timer1s += 0.1f;
                triggerMap[Trigger.EveryTick].Invoke(new Context());
            }
        }
    }

}

