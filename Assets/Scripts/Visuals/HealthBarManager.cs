using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public Entity linkedEntity;
    public GameObject bar;
    public GameObject shieldBar;

    public void Update()
    {
        if(linkedEntity.GetCarac(Carac.MaxHp) != 0)
        {
            float size = Mathf.Clamp01((float)linkedEntity.GetCarac(Carac.CurrentHp) / linkedEntity.GetCarac(Carac.MaxHp));
            bar.transform.localScale = new Vector3(size, 1, 1);
        }

        var shieldValue = linkedEntity.ComputeShieldValue();
        if(shieldValue > 0)
        {
            shieldBar.SetActive(true);
            float size = Mathf.Clamp01((float)shieldValue / linkedEntity.GetCarac(Carac.MaxHp));
            shieldBar.transform.localScale = new Vector3(size, 1, 1);
        }
        else
        {
            shieldBar.SetActive(false);
        }
        
    }

}
