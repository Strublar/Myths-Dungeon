using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBar : MonoBehaviour
{
    public Hero linkedHero;
    public GameObject bar;
    public SpriteRenderer barSprite;
    public bool isInit = false;

    public void Init()
    {
        if(isInit)
            return;
        
        isInit = true;
        barSprite.color = linkedHero.definition.resourceBarColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (linkedHero == null)
        {
            bar.SetActive(false);
            return;
        }

        bar.SetActive(true);
        Init();
        if (linkedHero.definition.maxResources == 0)
        {
            bar.SetActive(false);
            return;
        }

        bar.transform.localScale =
            new Vector3((float)linkedHero.resources / linkedHero.definition.maxResources, 1, 1);
    }
}