using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Stats
    [Header("Components")]
    public GameObject damagebubble;
    public GameObject healingBubble;
    public GameObject armorBubble;
    public GameObject passivePrefab;
    public List<Passive> passiveObjects;

    [Header("Stats")]
    public float maxHp;
    public float currentHp;
    public float armor;
    public float haste;
    public float damageModifier;

    [HideInInspector]
    public bool isAlive = true;


    #endregion

    public void DealDamage(float value)
    {
        if(isAlive)
        {
            float damageValue=0;
            if (armor >= 0)
                damageValue = value * (100 / (100 + armor));
            else
                damageValue = value * ((100-armor) / 100);
            currentHp -= damageValue;

            if (Mathf.RoundToInt(damageValue) > 0)
            {
                GameObject bubble = Instantiate(damagebubble, transform);
                bubble.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1.5f, -0.5f), 0);
                bubble.GetComponent<DamageBubbleBehaviour>().text.text = "-" + Mathf.RoundToInt(damageValue);
            }

            if (currentHp <= 0)
            {
                Die();
            }



            
        }
        
    }

    public void Heal(float value)
    {
        if(isAlive)
        {
            float healingValue = Mathf.Min(maxHp - currentHp, value);
            currentHp += healingValue;



            if (Mathf.RoundToInt(healingValue) > 0)
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
        foreach(Passive passive in passiveObjects)
        {
            if(passive != null)
            {
                passive.Delete(new Context());
            }
            
        }
        passiveObjects.Clear();
    }

}
