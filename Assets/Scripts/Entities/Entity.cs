using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

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

    #endregion

    public int GetCarac(Carac carac)
    {
        if (caracs.TryGetValue(carac, out int value))
        {
            return value;
        }

        return 0;
    }

    public void DealDamage(Context context)
    {
        if (isAlive)
        {
            int damageValue = 0;
            var armor = GetCarac(Carac.armor);
            if (armor >= 0)
                damageValue = Mathf.RoundToInt(context.value * (100.0f / (100 + armor)));
            else
                damageValue = Mathf.RoundToInt(context.value * ((100.0f - armor) / 100));
            caracs[Carac.currentHp] -= damageValue;

            if (damageValue > 0)
            {
                BubbleBehaviour bubble = Instantiate(damageBubble, transform);
                bubble.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1.5f, -0.5f), 0);
                bubble.text.text = "-" + Mathf.RoundToInt(damageValue) + (context.isCritical ? "!!!" : "");
            }

            TriggerManager.OnDamageReceived.Invoke(new Context()
            {
                source = context.source, target = this,
                value = damageValue,
                percentHpLost = Mathf.RoundToInt((float)damageValue / GetCarac(Carac.maxHp) * 100)
            });

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
}
