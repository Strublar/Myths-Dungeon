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
    [Header("Definition")]
    public HeroDefinition definition;
    public Item item;
    [HideInInspector]
    public float currentCooldown = 0f;
    [Header("FightStats")]
    public float attack;
    
    
    public float threat;

    [Header("Component objects")]
    public GameObject model;
    public GameObject healthBar;
    private bool isDragging;
    private Vector3 modelInitialPos;


    #endregion

    public void Start()
    {
        modelInitialPos = model.transform.localPosition;
    }

    public void Update()
    {
        currentCooldown -= Time.deltaTime*Mathf.Max(haste/100,0);
        if(Input.touchCount>0)
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

        if(currentCooldown>0&& GameManager.gm.fightStarted)
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
        isAlive = true;
        model.SetActive(true);
        healthBar.SetActive(true);
        ClearPassives();

        maxHp = definition.hp+definition.hpPerLevel*level;
        armor = definition.armor+definition.armorPerLevel * level;
        attack = definition.attack + definition.attackPerLevel * level;
        haste = 100+3*level;
        damageModifier = 100;
        threat = 0;
        currentCooldown = 0;
        foreach (PassiveDefinition passive in definition.passives)
        {
            GameObject newPassive = Instantiate(passivePrefab, this.transform);
            Passive pass = newPassive.GetComponent<Passive>();
            pass.holder = this;
            pass.definition = passive;
            pass.level = level;
            passiveObjects.Add(pass);
        }

        if(item.definition != null)
        {
            if (item.quality == ItemQuality.legacy)
                item.level = level;
            //Stats
            maxHp += item.definition.hp + item.definition.hpPerLevel * item.level;
            armor += item.definition.armor + item.definition.armorPerLevel * item.level;
            attack += item.definition.attack + item.definition.attackPerLevel * item.level;
            haste += item.definition.haste + item.definition.hastePerLevel * item.level;
            damageModifier += item.definition.damageModifier + item.definition.damageModifierPerLevel * item.level;
            //Passives
            foreach (PassiveDefinition passive in item.definition.passives)
            {
                GameObject newPassive = Instantiate(passivePrefab, this.transform);
                Passive pass = newPassive.GetComponent<Passive>();
                pass.holder = this;
                pass.definition = passive;
                pass.level = item.level;
                passiveObjects.Add(pass);
            }
        }

        currentHp = maxHp;
    }

    public void OnTap()
    {
        isDragging = false;
        if(GameManager.gm.isDeckbuilding)
        {
            GameManager.gm.ShowHeroes(definition.type);
        }
        else if (!GameManager.gm.fightStarted)
        {
            HeroTooltipManager.instance.InitHeroTooltip(this);
            HeroTooltipManager.instance.ShowToolTip();
        }
    }

    public void OnDrag(GameObject target)
    {
        if(isAlive && currentCooldown<=0 )
        {
            if (!definition.isHealer)
            {
                if (target.CompareTag("Boss"))
                {
                    Pull(target);
                    target.SendMessage("DealDamage", attack*Mathf.Max(damageModifier/100,0), SendMessageOptions.DontRequireReceiver);
                    Context context;
                    context.source = this;
                    context.target = target.GetComponent<Entity>();
                    context.passiveHolder = null;
                    TriggerManager.triggerMap[Trigger.OnAttack].Invoke(context);
                    threat += attack * Mathf.Max(damageModifier / 100, 0) * definition.threatRatio;
                    currentCooldown = definition.attackCooldown;
                    
                }
            }
            else
            {
                if (target.CompareTag("Hero") && GameManager.gm.fightStarted )
                {
                    Pull(target);
                    target.SendMessage("Heal", definition.attack * Mathf.Max(damageModifier / 100, 0), SendMessageOptions.DontRequireReceiver);
                    Context context;
                    context.source = this;
                    context.target = target.GetComponent<Entity>();
                    context.passiveHolder = null;
                    TriggerManager.triggerMap[Trigger.OnHealed].Invoke(context);
                    threat += attack * Mathf.Max(damageModifier / 100, 0) * definition.threatRatio;
                    currentCooldown = definition.attackCooldown;
                    
                }
            }

            if (GameManager.gm.mostThreatHero == null)
            {
                GameManager.gm.mostThreatHero = this;
            }
            if (threat >= GameManager.gm.mostThreatHero.threat)
            {
                GameManager.gm.mostThreatHero = this;
            }
            
        }
        isDragging = false;
        
    }

    public void Pull(GameObject target)
    {
        if (!GameManager.gm.fightStarted)
        {
            GameManager.gm.fightStarted = true;
            GameManager.gm.bossTimer = Time.time;
            Context context;
            context.source = this;
            context.target = target.GetComponent<Entity>();
            context.passiveHolder = null;
            TriggerManager.triggerMap[Trigger.OnPull].Invoke(context);
        }
    }
    public void OnStartDragging()
    {
        if(currentCooldown<=0)
            isDragging = true;
    }

    public void OnStayedHovered()
    {
        HeroTooltipManager.instance.InitHeroTooltip(this);
        HeroTooltipManager.instance.ShowToolTip();
        isDragging = false;
    }

    public override void Die()
    {
        base.Die();
        model.SetActive(false);
        healthBar.SetActive(false);
        GameManager.gm.GetNewThreatHero();
        GameManager.gm.HeroDies();
    }
}
