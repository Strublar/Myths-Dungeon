using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBar : MonoBehaviour
{

    public Hero linkedHero;
    public BossSpellDefinition linkedSpell;
    public GameObject bar;
    // Update is called once per frame
    void Update()
    {
        
        if(!GameManager.gm.fightStarted)
        {
            bar.SetActive(false);
            return;
        }
            
        if (linkedHero != null)
        {
            if(linkedHero.currentCooldown <=0)
            {
                bar.SetActive(false);
            }
            else
            {
                bar.SetActive(true);
                bar.transform.localScale = new Vector3(linkedHero.currentCooldown / linkedHero.definition.attackCooldown, 1, 1);
            }
        }

        if(linkedSpell != null)
        {
            if(linkedSpell.currentCooldown<=0)
            {
                bar.SetActive(false);
            }
            else
            {
                bar.SetActive(true);
                bar.transform.localScale = new Vector3(linkedSpell.currentCooldown / linkedSpell.coolDown, 1, 1);
            }
        }
    }
}
