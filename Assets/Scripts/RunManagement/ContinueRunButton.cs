using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ContinueRunButton : MonoBehaviour
{
    public static List<string> scenesToUnload = new List<string>();
    public void OnTap()
    {
        foreach (var scene in scenesToUnload)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        scenesToUnload.Clear();
        
        RunManager.instance.StartNewBoss();
        SceneManager.UnloadSceneAsync("ContinueScene");
    }
}
