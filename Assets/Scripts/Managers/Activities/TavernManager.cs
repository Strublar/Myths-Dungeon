using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TavernManager : MonoBehaviour
{
    public static TavernManager instance;

    public List<HeroDefinition> availableHeroes;
    public List<ChooseHeroButton> tavernSlots;
    
    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        RollHeroes();
        ContinueRunButton.scenesToUnload.Add("TavernScene");
        SceneManager.LoadScene("ContinueScene", LoadSceneMode.Additive);
    }

    private void RollHeroes()
    {
        var heroType = GetHeroTypes();
        foreach (var heroButton in tavernSlots)
        {
            HeroDefinition definition;
            do
            {
                definition = availableHeroes[Random.Range(0, availableHeroes.Count)];
            } while (!heroType.Contains(definition.type));

            heroButton.hero.definition = definition;
            //heroButton.hero.skill = definition.availableSkills[Random.Range(0, definition.availableSkills.Count)];
            heroButton.hero.ability = definition.baseAbility;
            heroButton.hero.LoadDefinition();
        }
    } 

    private List<HeroType> GetHeroTypes()
    {
        switch(HubManager.chosenActivity)
        {
            case Activity.TavernTank:
                return new List<HeroType>() { HeroType.Tank };
            case Activity.TavernDPS:
                return new List<HeroType>() { HeroType.DPS };
            case Activity.TavernHeal:
                return new List<HeroType>() { HeroType.Heal };
            case Activity.TavernEarly:
                return HubManager.remainingVagabonds;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void HeroChosen()
    {
        ContinueRunButton.scenesToUnload.Remove("TavernScene");
        SceneManager.UnloadSceneAsync("TavernScene");
    }
}
