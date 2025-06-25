using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePool : MonoBehaviour
{
    public static PassivePool instance;

    public Passive prefab;
    public int initialPoolingSize = 30;
    public Transform container;
    
    private Queue<Passive> pool = new Queue<Passive>();

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < initialPoolingSize; i++)
        {
            AddObjectToPool();
        }
    }

    private Passive AddObjectToPool()
    {
        Passive obj = Instantiate(prefab, container);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;    
    }
    
    
    public Passive GetObject(Transform parent)
    {
        if (pool.Count == 0)
        {
            AddObjectToPool();
        }

        Passive obj = pool.Dequeue();
        obj.transform.SetParent(parent,false);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(Passive obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(container,false);
        pool.Enqueue(obj);
    }
}