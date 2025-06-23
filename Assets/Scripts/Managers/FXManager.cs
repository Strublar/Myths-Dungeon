using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{

    public static FXManager instance;
    public GameObject healingParticlesPrefab;
    
    
    private void Awake()
    {
        instance = this;
    }
    
    
}
