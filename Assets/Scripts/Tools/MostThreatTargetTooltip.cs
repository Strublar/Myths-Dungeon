using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostThreatTargetTooltip : MonoBehaviour
{

    public GameObject model;
    public void Update()
    {
        model.SetActive(false);
        if (RunManager.instance.fightStarted)
        {
            if(FightManager.instance.mostThreatHero != null)
            {
                model.SetActive(true);
                transform.parent = FightManager.instance.mostThreatHero.transform;
                transform.localPosition = Vector3.zero;
            }
        }
        
        
    }

    private void OnDisable()
    {
        model.SetActive(false);
    }


}
