using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueRunButton : MonoBehaviour
{
    public void OnTap()
    {
        RunManager.instance.StartNewBoss();
        SceneManager.UnloadSceneAsync("ContinueScene");
    }
}
