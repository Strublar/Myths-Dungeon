using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Trigger
{
    Never,
    OnPull,
    OnAttack,
    OnHealed,
    EverySecond,
    EveryPersonalSecond,
    FightEnd,
}

public struct Context
{
    public Entity source;
    public Entity target;
    public Entity passiveHolder;
    
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

    public static Dictionary<Trigger, UnityEvent<Context>> triggerMap = new Dictionary<Trigger, UnityEvent<Context>>
    {
        { Trigger.Never,Never},
        { Trigger.OnPull,OnPull},
        { Trigger.OnAttack,OnAttack},
        { Trigger.OnHealed,OnHealed},
        { Trigger.EverySecond,EverySecond},
        { Trigger.EveryPersonalSecond,EveryPersonalSecond},
        { Trigger.FightEnd,FightEnd},


    };

    private float timer1s = 1f;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if(GameManager.gm.fightStarted)
        {
            timer1s -= Time.deltaTime;
            if(timer1s<0)
            {
                timer1s = 1f;
                triggerMap[Trigger.EverySecond].Invoke(new Context());
            }
        }
    }

}


