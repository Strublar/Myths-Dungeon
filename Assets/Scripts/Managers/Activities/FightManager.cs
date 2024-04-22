using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;

    public List<EnemyDefinition> bossPool;
    public Enemy boss;
    public Hero mostThreatHero = null;
    public int deadHeroes;
    public float bossTimer = 0f;
    public TextMeshPro currentLevel;
    public string lastAbilityName = "";

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        var bossBeaten = RunManager.instance.bossBeaten;
        currentLevel.text = "Boss : " + (bossBeaten + 1);
        bossTimer = 0;
        boss.level = bossBeaten + 1;

        do
        {
            boss.definition = bossPool[Random.Range(0, bossPool.Count)];
        } while (boss.definition == RunManager.instance.lastBoss && bossPool.Count != 1);


        RunManager.instance.lastBoss = boss.definition;
        boss.LoadDefinition();
        boss.gameObject.SetActive(true);
        deadHeroes = 0;
        lastAbilityName = "";

        RunManager.instance.ReloadHeroes();
    }


    public void GetNewThreatHero()
    {
        float maxThreat = -1;
        foreach (Hero hero in RunManager.instance.heroes)
        {
            if (hero.threat > maxThreat && hero.isAlive)
            {
                mostThreatHero = hero;
                maxThreat = mostThreatHero.threat;
            }
        }
    }

    public void UpdateMostThreatHero(Hero hero)
    {
        if (mostThreatHero == null)
        {
            mostThreatHero = hero;
        }

        if (hero.threat >= mostThreatHero.threat)
        {
            mostThreatHero = hero;
        }
    }

    public void ResetThreat()
    {
        foreach (var hero in RunManager.instance.heroes)
        {
            hero.threat = 0;
        }
    }

    public void HeroDies()
    {
        deadHeroes++;
        if (deadHeroes == RunManager.instance.heroes.Count)
        {
            //loseScreen.SetActive(true);
            //fightStarted = false;
            Debug.Log("TODO : end of run");
        }
    }
}