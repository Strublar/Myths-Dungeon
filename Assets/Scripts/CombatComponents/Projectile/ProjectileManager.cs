using System;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager instance;
    public Projectile projectilePrefab;
    public Transform container;
    
    public void Awake()
    {
        instance = this;
    }
}