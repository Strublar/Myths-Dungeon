using System.Collections.Generic;
using JetBrains.Annotations;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    public int level;
    public EnemyDefinition definition;
    [CanBeNull] public TextMeshPro enemyNameMesh;
    
    public GameObject model;
    
    public PassiveDefinition bossFrenzy;
    
    public void LoadDefinition()
    {
        ClearPassives();
        caracs[Carac.maxHp] = definition.hp + definition.hpPerLevel*level;
        if(enemyNameMesh != null)
            enemyNameMesh.text = definition.enemyName;
        isAlive = true;
        foreach(EnemySpellDefinition spell in definition.spells)
        {
            spell.currentCooldown = spell.coolDown;
        }

        model = Instantiate(definition.model, transform);
        
        foreach (PassiveDefinition passive in definition.passives)
        {
            Passive newPassive = Instantiate(passivePrefab, this.transform);
            newPassive.holder = this;
            newPassive.definition = passive;
            newPassive.level = level;
            passives.Add(newPassive);
        }
        if(level>=10)//creep //TODO Cible, casting bar
        {
            caracs[Carac.maxHp] *= Mathf.RoundToInt(Mathf.Pow(1.1f, level - 10));
            Passive newPassive = Instantiate(passivePrefab, this.transform);
            newPassive.holder = this;
            newPassive.definition = bossFrenzy;
            newPassive.level = level;
            passives.Add(newPassive);
        }
        caracs[Carac.currentHp] = GetCarac(Carac.maxHp);

    }


    public void Update()
    {
        if (RunManager.instance.fightStarted && isAlive)
        {
            foreach(EnemySpellDefinition spell in definition.spells)
            {
                if(spell.minLevel <= level)
                {
                    spell.currentCooldown -= Time.deltaTime;
                    if (spell.currentCooldown < 0)
                    {
                        Cast(spell);
                        spell.currentCooldown = spell.coolDown;
                    }
                }
                
            }
            
        }
    }
    public void Cast(EnemySpellDefinition spell)
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
                context.target = ent;
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
        Destroy(model);
        model = null;
        gameObject.SetActive(false);
        if(definition.isBoss)
            RunManager.instance.BossDefeated();
    }
}
