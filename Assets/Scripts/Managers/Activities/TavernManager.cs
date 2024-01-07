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
        var heroType = GetHeroType();
        foreach (var heroButton in tavernSlots)
        {
            HeroDefinition definition;
            do
            {
                definition = availableHeroes[Random.Range(0, availableHeroes.Count)];
            } while (definition.type != heroType);

            heroButton.hero.definition = definition;
            heroButton.hero.skill = definition.availableSkills[Random.Range(0, definition.availableSkills.Count)];
            heroButton.hero.ability = definition.availableAbilities[Random.Range(0, definition.availableAbilities.Count)];
            heroButton.hero.LoadDefinition();
        }
    }

    private HeroType GetHeroType()
    {
        switch(HubManager.chosenActivity)
        {
            case Activity.TavernTank:
                return HeroType.Tank;
            case Activity.TavernDPS:
                return HeroType.DPS;
            case Activity.TavernHeal:
                return HeroType.Heal;
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
