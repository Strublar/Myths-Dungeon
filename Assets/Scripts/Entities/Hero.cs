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
    public PassiveDefinition skill;
    public AbilityDefinition ability;

    [Header("FightStats")] public float threat;
    public int resources;
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
        passives.Add(skill);
        passives.Add(ability.linkedPassive);
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

        //resource passive
        GameObject resourcePassiveObject = Instantiate(passivePrefab, transform);
        Passive resourcePassive = resourcePassiveObject.GetComponent<Passive>();
        resourcePassive.holder = this;
        resourcePassive.definition = PassiveDefinition.BuildResourcePassive(this);
        resourcePassive.level = item.level;
        passiveObjects.Add(resourcePassive);


        attackContext = new Context
        {
            passiveHolder = this, source = this
        };

        resources = definition.startResources;
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
            if (!definition.isHealer)
            {
                if (target.CompareTag("Boss"))
                {
                    Pull(target);
                    Context context = new Context
                    {
                        source = this,
                        target = target.GetComponent<Entity>(),
                        passiveHolder = null,
                        isCritical = Random.Range(0, 100) <= critChance
                    };
                    TriggerManager.triggerMap[Trigger.OnAttack].Invoke(context);
                    resources += ability.resourceModification;
                }
            }
            else
            {
                if (target.CompareTag("Hero") && RunManager.instance.fightStarted)
                {
                    Pull(target);
                    Context context = new Context
                    {
                        source = this,
                        target = target.GetComponent<Entity>(),
                        passiveHolder = null,
                        isCritical = Random.Range(0, 100) <= critChance
                    };
                    TriggerManager.triggerMap[Trigger.OnHeal].Invoke(context);
                    resources += ability.resourceModification;
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
        int resourcePreview = resources + ability.resourceModification;
        bool canCast = resourcePreview >= 0 &&
                       (resourcePreview <= definition.maxResources || definition.canResourceOverflow);
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