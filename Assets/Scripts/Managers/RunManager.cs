using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = System.Random;

public class RunManager : MonoBehaviour
{
    public static RunManager instance;
    public List<Hero> heroes;
    public bool fightStarted;
    public int bossBeaten = 0;
    public EnemyDefinition lastBoss;
    [Header("Components")] public GameObject loseScreen;
    public TextMeshPro victoryText;

    public List<HeroType> RemainingVagabonds
    {
        get
        {
            var remainingVagabonds = new List<HeroType>();
            foreach (var hero in instance.heroes)
            {
                if (hero.definition.isVagabond)
                {
                    if (!remainingVagabonds.Contains(hero.definition.type))
                    {
                        remainingVagabonds.Add(hero.definition.type);
                    }
                }
            }

            return remainingVagabonds;
        }
    }


    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        StartNewRun();
    }

    public void StartNewBoss()
    {
        victoryText.gameObject.SetActive(false);
        SceneManager.LoadScene("FightScene", LoadSceneMode.Additive);
    }

    public void BossDefeated()
    {
        bossBeaten++;
        float killTime = Time.time - FightManager.instance.bossTimer;
        SceneManager.UnloadSceneAsync("FightScene");
        SceneManager.LoadScene("SkillScene", LoadSceneMode.Additive);
        victoryText.text = "Victory!!!\nTime : " + string.Format("{0:0.#}", killTime) + "s";
        victoryText.gameObject.SetActive(true);
        fightStarted = false;

        foreach (Hero hero in heroes)
        {
            hero.LoadDefinition();
        }
    }


    public void StartNewRun()
    {
        loseScreen.SetActive(false);
        bossBeaten = 0;
        foreach (Hero hero in heroes)
        {
            hero.item = null;
        }

        ReloadHeroes();
        StartNewBoss();
    }

    public void ReturnToMenu()
    {
        loseScreen.SetActive(false);
        GameManager.instance.LoadMenu();
    }


    public void ReloadHeroes()
    {
        foreach (Hero hero in heroes)
        {
            hero.LoadDefinition();
        }
    }

    public Dictionary<SkillTag,int> GetAllSkillTags()
    {
        Dictionary<SkillTag,int> tags = new();

        foreach (var hero in heroes)
        {
            foreach (var skillTagPair in hero.skillTags)
            {
                if (tags.ContainsKey(skillTagPair.Key))
                {
                    tags[skillTagPair.Key] += skillTagPair.Value;
                }
                else
                {
                    tags[skillTagPair.Key] = skillTagPair.Value;
                }
            }
        }

        return tags;
    }
}