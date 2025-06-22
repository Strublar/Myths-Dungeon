using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Camera mainCamera;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadMenu();
    }


    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
    }

    public void StartNewRun()
    {
        SceneManager.UnloadSceneAsync("MenuScene");
        SceneManager.LoadScene("RunScene", LoadSceneMode.Additive);
    }
}
