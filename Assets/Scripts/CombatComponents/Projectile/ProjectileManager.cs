using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager instance;
    public Projectile projectilePrefab;
    public Transform container;
    public int initialPoolingSize = 10;
    
    private Queue<Projectile> pool = new Queue<Projectile>();

    public void Awake()
    {
        instance = this;
        for (int i = 0; i < initialPoolingSize; i++)
        {
            AddObjectToPool();
        }
    }
    
    
    private Projectile AddObjectToPool()
    {
        Projectile obj = Instantiate(projectilePrefab, container);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }
    
    
    public Projectile GetObject()
    {
        if (pool.Count == 0)
        {
            AddObjectToPool();
        }

        Projectile obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(Projectile obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}