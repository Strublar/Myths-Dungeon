using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Hero> heroes;
    public List<BossDefinition> bossPool;
    public Boss boss;
    public Hero mostThreatHero = null;
    public bool fightStarted;
    public int bossBeaten = 0;
    public int deadHeroes;
    [Header("Components")]
    public GameObject victoryScreen;
    public GameObject loseScreen;
    public TextMeshPro currentLevel;
    public GameObject menuObject;
    public GameObject deckbuilder, tankOptions, healOptions,dpsOptions, deckbuilderMenu;
    public TextMeshPro victoryText;
    public bool isDeckbuilding = false;
    public HeroType currentChoice;
    public float bossTimer= 0f;
    public void Awake()
    {
        gm = this;
    }
    public void Start()
    {
        //StartNewBoss();
        foreach (Hero hero in heroes)
        {
            hero.level = 1;
            hero.item.definition = null;
            hero.LoadDefinition();
        }
    }

    public void StartNewBoss()
    {
        currentLevel.text = "Level : " + (bossBeaten + 1);
        victoryScreen.SetActive(false);
        bossTimer = 0;
        boss.level = bossBeaten+1;
        if (boss.model != null)
            Destroy(boss.model);
        BossDefinition lastBoss = boss.definition;
        do
        {
            boss.definition = bossPool[Random.Range(0, bossPool.Count)];
        } while (boss.definition == lastBoss && bossPool.Count != 1);
        boss.LoadDefinition();
        boss.gameObject.SetActive(true);
        deadHeroes = 0;
        foreach (Hero hero in heroes)
        {

            hero.LoadDefinition();
        }

    }

    public void BossDefeated()
    {
        bossBeaten++;
        victoryScreen.SetActive(true);
        Destroy(boss.model);
        float killTime = Time.time - bossTimer;
        Debug.Log("killTime = " + killTime);
        victoryText.text = "Victory!!!\nTime : " + string.Format("{0:0.#}",killTime) +"s";
        fightStarted = false;
        mostThreatHero = null;
        foreach (Hero hero in heroes)
        {
            hero.AddXp(1);
            hero.LoadDefinition();
        }

    }

    public void GetNewThreatHero()
    {
        float maxThreat = -1;
        foreach(Hero hero in heroes)
        {
            if(hero.threat > maxThreat && hero.isAlive)
            {
                mostThreatHero = hero;
                maxThreat = mostThreatHero.threat;
            }
        }
    }

    public void HeroDies()
    {
        deadHeroes++;
        if(deadHeroes == 7)
        {
            loseScreen.SetActive(true);
            fightStarted = false;
        }
    }

    public void StartNewRun()
    {
        menuObject.SetActive(false);
        loseScreen.SetActive(false);
        bossBeaten = 0;
        foreach (Hero hero in heroes)
        {

            hero.level = 1;
            hero.item.definition = null;
        }
        StartNewBoss();
    }

    public void ReturnToMenu()
    {
        loseScreen.SetActive(false);
        menuObject.SetActive(true);
        boss.gameObject.SetActive(false);
        deckbuilder.SetActive(false);
        isDeckbuilding = false;
        foreach (Hero hero in heroes)
        {
            hero.level = 0;
            hero.item.definition = null;
            hero.LoadDefinition();
        }
    }

    public void ChangeRoster()
    {
        menuObject.SetActive(false);
        deckbuilder.SetActive(true);
        isDeckbuilding = true;
        tankOptions.SetActive(false);
        dpsOptions.SetActive(false);
        healOptions.SetActive(false);
        deckbuilderMenu.SetActive(true);
        currentChoice = HeroType.none;
    }

    public void ShowHeroes(HeroType type)
    {
        if(currentChoice == type)
        {
            ChangeRoster();
            return;
        }
        currentChoice = type;
        deckbuilderMenu.SetActive(false);
        switch (type)
        {
            case HeroType.Tank:
                tankOptions.SetActive(true);
                dpsOptions.SetActive(false);
                healOptions.SetActive(false);
                break;
            case HeroType.Heal:
                healOptions.SetActive(true);
                tankOptions.SetActive(false);
                dpsOptions.SetActive(false);
                break;
            case HeroType.DPS:
                dpsOptions.SetActive(true);
                tankOptions.SetActive(false);
                healOptions.SetActive(false);
                break;
        }
    }

    public void UpdateMostThreatHero(Hero hero)
    {
        if (mostThreatHero == null)
        {
            mostThreatHero = hero;
        }

        if (hero.threat >= GameManager.gm.mostThreatHero.threat)
        {
            mostThreatHero = hero;
        }
    }

    public void ResetThreat()
    {
        foreach (var hero in heroes)
        {
            hero.threat = 0;
        }
    }
}
