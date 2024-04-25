using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBar : MonoBehaviour
{
    public Hero linkedHero;
    public GameObject bar;
    public SpriteRenderer barSprite;


    // Update is called once per frame
    void Update()
    {
        if (linkedHero == null)
        {
            bar.SetActive(false);
            return;
        }

        bar.SetActive(true);
        barSprite.color = linkedHero.definition.cooldownBarColor;

        bar.transform.localScale =
            new Vector3(Mathf.Max(linkedHero.currentAbilityCooldown,0) / linkedHero.ability.cooldown, 1, 1);
    }
}