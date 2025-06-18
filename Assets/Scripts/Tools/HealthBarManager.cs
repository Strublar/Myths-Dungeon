using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public Entity linkedEntity;
    public GameObject bar;

    public void Update()
    {
        if(linkedEntity.GetCarac(Carac.maxHp) != 0)
        {
            float size = Mathf.Clamp01((float)linkedEntity.GetCarac(Carac.currentHp) / linkedEntity.GetCarac(Carac.maxHp));
            bar.transform.localScale = new Vector3(size, 1, 1);
        }
        
    }

}
