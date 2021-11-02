using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseHeroButton : MonoBehaviour
{
    public Hero hero;
    public void OnTap()
    {

    }

    public void OnStayedHovered()
    {
        hero.OnStayedHovered();
    }

    public void OnDrag(GameObject target)
    {
        if (target.CompareTag("Hero"))
        {
            Hero targetHero = target.GetComponent<Hero>();
            if(targetHero.definition.type == hero.definition.type)
            {
                targetHero.definition = hero.definition;
                targetHero.LoadDefinition();
            }
            
        }
    }

    public void OnEnable()
    {
        hero.LoadDefinition();
    }
    public void OnDisable()
    {
        hero.ClearPassives();
    }
}
