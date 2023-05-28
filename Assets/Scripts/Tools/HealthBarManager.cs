using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public Entity linkedEntity;
    public GameObject bar;

    public void Update()
    {
        if(linkedEntity.maxHp != 0)
        {
            float size = Mathf.Clamp01((float)linkedEntity.currentHp / linkedEntity.maxHp);
            bar.transform.localScale = new Vector3(size, 1, 1);
        }
        
    }

}
