using System;
using System.Collections.Generic;
using CombatComponents.Shields;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    #region Stats

    public BubbleBehaviour damageBubble;
    public BubbleBehaviour healingBubble;
    [FormerlySerializedAs("passiveObjects")] public List<Passive> passives;

    [Header("Stats")] public Dictionary<Carac, int> caracs = new();
    [HideInInspector] public bool isAlive = true;

    public List<ShieldData> shieldValues = new();
    private readonly List<ShieldData> shieldsToRemove = new();

    #endregion


    public void Update()
    {
        var enumerator = shieldValues.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var shieldData = enumerator.Current;
            var newDuration = shieldData.remainingDuration - Time.deltaTime;

            if (newDuration > 0)
            {
                shieldData.UpdateDuration(newDuration);
            }
            else
            {
                shieldsToRemove.Add(shieldData);
            }
        }

        ClearShields();
        enumerator.Dispose();
    }

    public int GetCarac(Carac carac)
    {
        if (caracs.TryGetValue(carac, out int value))
        {
            return value;
        }

        return 0;
    }

    public virtual void DealDamage(Context context)
    {
        if (isAlive)
        {
            int damageValue = 0;
            var armor = GetCarac(Carac.armor);
            if (armor >= 0)
                damageValue = Mathf.RoundToInt(context.value * (100.0f / (100 + armor)));
            else
                damageValue = Mathf.RoundToInt(context.value * ((100.0f - armor) / 100));

            //Handle shields
            var shieldEnumerator = shieldValues.GetEnumerator();
            while (shieldEnumerator.MoveNext())
            {
                var shieldData = shieldEnumerator.Current;
                if (shieldData.remainingValue > damageValue)
                {
                    shieldData.UpdateValue(shieldData.remainingValue - damageValue);
                    damageValue = 0;
                }
                else
                {
                    shieldsToRemove.Add(shieldData);
                }
            }

            ClearShields();
            shieldEnumerator.Dispose();

            //Apply damage
            caracs[Carac.currentHp] -= damageValue;

            if (damageValue > 0)
            {
                BubbleBehaviour bubble = Instantiate(damageBubble, transform);
                bubble.transform.localPosition = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, -50f), 0);
                bubble.text.text = "-" + Mathf.RoundToInt(damageValue) + (context.isCritical ? "!!!" : "");

                TriggerManager.OnDamageReceived.Invoke(new Context()
                {
                    source = context.source, target = this,
                    value = damageValue,
                    percentHpLost = Mathf.RoundToInt((float)damageValue / GetCarac(Carac.maxHp) * 100)
                });
            }

            if (GetCarac(Carac.currentHp) <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(Context context)
    {
        if (isAlive)
        {
            int healingValue = Mathf.RoundToInt(Mathf.Min(GetCarac(Carac.maxHp) - GetCarac(Carac.currentHp), context.value));
            caracs[Carac.currentHp] += healingValue;

            TriggerManager.triggerMap[Trigger.OnHeal].Invoke(context);
            if (healingValue > 0)
            {
                BubbleBehaviour bubble = Instantiate(healingBubble, transform);
                bubble.transform.localPosition = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, -50f), 0);
                bubble.text.text = "+" + Mathf.RoundToInt(healingValue) + (context.isCritical ? "!!!" : "");
            } 
        }
    }

    public void AddShield(int shieldValue, float duration)
    {
        shieldValues.Add(new ShieldData(shieldValue, duration));
    }

    public int ComputeShieldValue()
    {
        int shieldValue = 0;
        foreach (var shieldData in shieldValues)
        {
            shieldValue += shieldData.remainingValue;
        }

        return shieldValue;
    }


    public virtual void Die()
    {
        isAlive = false;
        ClearPassives();
    }

    public void ClearPassives()
    {
        foreach (Passive passive in passives)
        {
            if (passive != null)
            {
                passive.Delete(new Context());
            }
        }

        shieldsToRemove.AddRange(shieldValues);
        ClearShields();
        passives.Clear();
    }

    public void ClearShields()
    {
        foreach (var shieldData in shieldsToRemove)
        {
            shieldValues.Remove(shieldData);
        }

        shieldsToRemove.Clear();
    }
}