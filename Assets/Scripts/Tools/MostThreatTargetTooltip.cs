using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostThreatTargetTooltip : MonoBehaviour
{

    public GameObject model;
    public void Update()
    {
        model.SetActive(false);
        if (GameManager.gm.fightStarted)
        {
            if(GameManager.gm.mostThreatHero != null)
            {
                model.SetActive(true);
                transform.parent = GameManager.gm.mostThreatHero.transform;
                transform.localPosition = Vector3.zero;
            }
        }
        
        
    }

    private void OnDisable()
    {
        model.SetActive(false);
    }


}
