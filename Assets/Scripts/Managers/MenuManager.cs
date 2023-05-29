using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnStartNewRun()
    {
        GameManager.instance.StartNewRun();
    }
}
