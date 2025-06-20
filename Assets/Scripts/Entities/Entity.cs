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
    public BubbleBehaviour armorBubble;
    public GameObject passivePrefab;
    public List<Passive> passiveObjects;

    [Header("Stats")] public Dictionary<Carac, int> caracs = new();
    [HideInInspector] public bool isAlive = true;

    public List<ShieldData> ShieldValues = new();
    private readonly List<ShieldData> _shieldsToRemove = new();

    #endregion


    public void Update()
    {
        var enumerator = ShieldValues.GetEnumerator();
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
                _shieldsToRemove.Add(shieldData);
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
            var shieldEnumerator = ShieldValues.GetEnumerator();
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
                    _shieldsToRemove.Add(shieldData);
                }
            }

            ClearShields();
            shieldEnumerator.Dispose();

            //Apply damage
            caracs[Carac.currentHp] -= damageValue;

            if (damageValue > 0)
            {
                BubbleBehaviour bubble = Instantiate(damageBubble, transform);
                bubble.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1.5f, -0.5f), 0);
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

    public void Heal(int value, bool isCritical)
    {
        if (isAlive)
        {
            int healingValue = Mathf.RoundToInt(Mathf.Min(GetCarac(Carac.maxHp) - GetCarac(Carac.currentHp), value));
            caracs[Carac.currentHp] += healingValue;

            if (healingValue > 0)
            {
                BubbleBehaviour bubble = Instantiate(healingBubble, transform);
                bubble.transform.localPosition = new Vector3(Random.Range(-.6f, .6f), Random.Range(-1.5f, -0.5f), 0);
                bubble.text.text = "+" + Mathf.RoundToInt(healingValue) + (isCritical ? "!!!" : "");
            }
        }
    }

    public void AddShield(int shieldValue, float duration)
    {
        ShieldValues.Add(new ShieldData(shieldValue, duration));
    }

    public int ComputeShieldValue()
    {
        int shieldValue = 0;
        foreach (var shieldData in ShieldValues)
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
        foreach (Passive passive in passiveObjects)
        {
            if (passive != null)
            {
                passive.Delete(new Context());
            }
        }

        passiveObjects.Clear();
    }

    public void ClearShields()
    {
        foreach (var shieldData in _shieldsToRemove)
        {
            ShieldValues.Remove(shieldData);
        }

        _shieldsToRemove.Clear();
    }
}