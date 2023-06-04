using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    Tank,
    Heal,
    DPS,
    none
}

public class Hero : Entity
{
    #region Stats

    public int level;
    public int xp;

    [Header("Definition")] public HeroDefinition definition;
    public Item item;
    public SkillDefinition skill;
    public AbilityDefinition ability;

    [Header("FightStats")] [HideInInspector]
    public float currentCooldown = 0f;
    public float threat;
    public int critChance;
    public int critPower;

    [Header("Component objects")] public GameObject model;
    public GameObject healthBar;
    private bool isDragging;
    private Vector3 modelInitialPos;

    private Context attackContext;

    #endregion

    public void Update()
    {
        currentCooldown -= Time.deltaTime * Mathf.Max(haste / 100, 0);
        if (Input.touchCount > 0)
        {
            if (isDragging)
            {
                Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                newPos.z = 0;
                model.transform.position = newPos;
            }
        }
        else
        {
            isDragging = false;
            model.transform.localPosition = modelInitialPos;
        }

        if (!CanCast() && RunManager.instance.fightStarted)
            model.GetComponent<SpriteRenderer>().color = Color.gray;
        else
            model.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void LoadDefinition()
    {
        if (model != null)
            Destroy(model);
        model = Instantiate(definition.model, transform);
        model.transform.localPosition = new Vector3(.15f, -.5f);
        modelInitialPos = model.transform.localPosition;
        isAlive = true;
        model.SetActive(true);
        healthBar.SetActive(true);
        ClearPassives();

        maxHp = definition.hp + definition.hpPerLevel * level;
        armor = definition.armor + definition.armorPerLevel * level;
        haste = 100 + 3 * level;
        damageModifier = 100;
        threat = 0;
        critChance = definition.critChance + definition.critChancePerLevel * level;
        critPower = definition.critPower + definition.critPowerPerLevel * level;

        var passives = new List<PassiveDefinition>(definition.passives);
        
        foreach (var passive in skill.passives)
        {
            passives.Add(passive);
        }
        
        foreach (var passive in ability.linkedPassives)
        {
            passives.Add(passive);
        }

        foreach (PassiveDefinition passive in passives)
        {
            GameObject newPassive = Instantiate(passivePrefab, this.transform);
            Passive pass = newPassive.GetComponent<Passive>();
            pass.holder = this;
            pass.definition = passive;
            pass.level = level;
            passiveObjects.Add(pass);
        }

        if (item.definition != null)
        {
            if (item.quality == ItemQuality.legacy)
                item.level = level;
            //Stats
            maxHp += item.definition.hp + item.definition.hpPerLevel * item.level;
            armor += item.definition.armor + item.definition.armorPerLevel * item.level;
            haste += item.definition.haste + item.definition.hastePerLevel * item.level;
            damageModifier += item.definition.damageModifier + item.definition.damageModifierPerLevel * item.level;
            //Passives
            foreach (PassiveDefinition passive in item.definition.passives)
            {
                GameObject newPassive = Instantiate(passivePrefab, transform);
                Passive pass = newPassive.GetComponent<Passive>();
                pass.holder = this;
                pass.definition = passive;
                pass.level = item.level;
                passiveObjects.Add(pass);
            }
        }

        attackContext = new Context
        {
            passiveHolder = this, source = this
        };

        currentCooldown = 0f;
        currentHp = maxHp;
    }

    public void OnTap()
    {
        isDragging = false;
        if (!RunManager.instance.fightStarted)
        {
            HeroTooltipManager.instance.InitHeroTooltip(this);
            HeroTooltipManager.instance.ShowToolTip();
        }
    }

    public void OnDrag(GameObject target)
    {
        if (isAlive && CanCast())
        {
            if (!definition.IsHealer)
            {
                if (target.CompareTag("Boss"))
                {
                    currentCooldown = ability.cooldown;
                    Pull(target);
                    Context context = new Context
                    {
                        source = this,
                        target = target.GetComponent<Entity>(),
                        passiveHolder = null,
                        isCritical = Random.Range(0, 100) <= critChance,
                        level = level
                    };
                    TriggerManager.triggerMap[Trigger.OnAttack].Invoke(context);
                    FightManager.instance.lastAbilityName = ability.abilityName;
                }
            }
            else
            {
                if (target.CompareTag("Hero") && RunManager.instance.fightStarted)
                {
                    currentCooldown = ability.cooldown;
                    Pull(target);
                    Context context = new Context
                    {
                        source = this,
                        target = target.GetComponent<Entity>(),
                        passiveHolder = null,
                        isCritical = Random.Range(0, 100) <= critChance,
                        level = level
                    };
                    TriggerManager.triggerMap[Trigger.OnHeal].Invoke(context);
                    FightManager.instance.lastAbilityName = ability.abilityName;
                    
                }
            }

            if (FightManager.instance.mostThreatHero == null)
            {
                FightManager.instance.mostThreatHero = this;
            }

            if (threat >= FightManager.instance.mostThreatHero.threat)
            {
                FightManager.instance.mostThreatHero = this;
            }
        }

        isDragging = false;
    }

    public void Pull(GameObject target)
    {
        if (!RunManager.instance.fightStarted)
        {
            FightManager.instance.mostThreatHero = this;
            RunManager.instance.fightStarted = true;
            FightManager.instance.bossTimer = Time.time;
            Context context = new Context
            {
                source = this,
                target = target.GetComponent<Entity>(),
                passiveHolder = null
            };
            TriggerManager.triggerMap[Trigger.OnPull].Invoke(context);
        }
    }

    public void OnStartDragging()
    {
        if (CanCast())
            isDragging = true;
    }

    public void OnStayedHovered()
    {
        HeroTooltipManager.instance.InitHeroTooltip(this);
        HeroTooltipManager.instance.ShowToolTip();
        isDragging = false;
    }

    public bool CanCast()
    {
        bool canCast = currentCooldown <= 0;
        foreach (var condition in ability.castConditions)
        {
            canCast &= condition.ShouldTrigger(attackContext);
        }

        return canCast;
    }

    public override void Die()
    {
        base.Die();
        model.SetActive(false);
        healthBar.SetActive(false);
        FightManager.instance.GetNewThreatHero();
        FightManager.instance.HeroDies();
    }

    public void AddXp(int i)
    {
        xp += i;
        while (xp >= level)
        {
            xp -= level;
            level++;
        }
    }
}