using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Stats

    [Header("Components")] public GameObject damagebubble;
    public GameObject healingBubble;
    public GameObject armorBubble;
    public GameObject passivePrefab;
    public List<Passive> passiveObjects;

    [Header("Stats")] public int maxHp;
    public int currentHp;
    public int armor;
    public int haste;
    public int damageModifier;

    [HideInInspector] public bool isAlive = true;

    #endregion

    public void DealDamage(int value, Entity source)
    {
        if (isAlive)
        {
            int damageValue = 0;
            if (armor >= 0)
                damageValue = Mathf.RoundToInt(value * (100.0f / (100 + armor)));
            else
                damageValue = Mathf.RoundToInt(value * ((100.0f - armor) / 100));
            currentHp -= damageValue;

            if (damageValue > 0)
            {
                GameObject bubble = Instantiate(damagebubble, transform);
                bubble.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1.5f, -0.5f), 0);
                bubble.GetComponent<DamageBubbleBehaviour>().text.text = "-" + Mathf.RoundToInt(damageValue);
            }

            TriggerManager.OnDamageReceived.Invoke(new Context()
            {
                source = source, target = this,
                value = damageValue,
                percentHpLost = Mathf.RoundToInt((float)damageValue/maxHp*100)
            });

            if (currentHp <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int value)
    {
        if (isAlive)
        {
            int healingValue = Mathf.RoundToInt(Mathf.Min(maxHp - currentHp, value));
            currentHp += healingValue;

            if (healingValue > 0)
            {
                GameObject bubble = Instantiate(healingBubble, transform);
                bubble.transform.localPosition = new Vector3(Random.Range(-.6f, .6f), Random.Range(-1.5f, -0.5f), 0);
                bubble.GetComponent<DamageBubbleBehaviour>().text.text = "+" + Mathf.RoundToInt(healingValue);
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