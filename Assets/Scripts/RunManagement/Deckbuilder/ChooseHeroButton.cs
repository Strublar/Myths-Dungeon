using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseHeroButton : MonoBehaviour
{
    public Hero hero;

    public void OnDrag(GameObject target)
    {
        if (target.CompareTag("Hero"))
        {
            Hero targetHero = target.GetComponent<Hero>();
            if(targetHero.definition.type == hero.definition.type)
            {
                targetHero.definition = hero.definition;
                targetHero.ability = hero.ability;
                //targetHero.skill = hero.skill;
                targetHero.LoadDefinition();
                TavernManager.instance.HeroChosen();
            }
            
        }
    }

    public void OnDestroy()
    {
        hero.ClearPassives();
    }
}
