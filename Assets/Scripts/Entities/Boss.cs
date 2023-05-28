using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : Entity
{
    public int level;
    public BossDefinition definition;
    public TextMeshPro bossName;
    public GameObject model;
    public PassiveDefinition bossFrenzy;

    public void LoadDefinition()
    {
        ClearPassives();
        maxHp = definition.hp + definition.hpPerLevel*level;
        
        haste = 100;
        armor = definition.armor + definition.armorPerLevel * level;
        bossName.text = definition.bossName;
        isAlive = true;
        foreach(BossSpellDefinition spell in definition.spells)
        {
            spell.currentCooldown = spell.coolDown;
        }
        model = Instantiate(definition.model, transform);
        model.transform.localPosition = new Vector3(0, -1, 0);
        foreach (PassiveDefinition passive in definition.passives)
        {
            GameObject newPassive = Instantiate(passivePrefab, this.transform);
            Passive pass = newPassive.GetComponent<Passive>();
            pass.holder = this;
            pass.definition = passive;
            pass.level = level;
            passiveObjects.Add(pass);
        }
        if(level>=10)//creep //TODO Cible, casting bar
        {
            maxHp *= Mathf.RoundToInt(Mathf.Pow(1.1f, level - 10));
            GameObject newPassive = Instantiate(passivePrefab, this.transform);
            Passive pass = newPassive.GetComponent<Passive>();
            pass.holder = this;
            pass.definition = bossFrenzy;
            pass.level = level;
            passiveObjects.Add(pass);
        }
        currentHp = maxHp;

    }


    public void Update()
    {
        if (GameManager.gm.fightStarted && isAlive)
        {
            foreach(BossSpellDefinition spell in definition.spells)
            {
                if(spell.minLevel <= level)
                {
                    spell.currentCooldown -= Time.deltaTime*haste/100;
                    if (spell.currentCooldown < 0)
                    {
                        Cast(spell);
                        spell.currentCooldown = spell.coolDown;
                    }
                }
                
            }
            
        }
    }
    public void Cast(BossSpellDefinition spell)
    {
        List<Entity> spellTargets = spell.targets.GetTargets(new Context());
        var context = new Context
        {
            passiveHolder = this,
            source = this,
            level = level
        };
        foreach(Entity ent in spellTargets)
        {
            if(ent != null)
            {
                foreach (Effect eff in spell.effects)
                {
                    eff.Apply(context);
                }
            }
            
        }
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
        GameManager.gm.BossDefeated();
    }
}
