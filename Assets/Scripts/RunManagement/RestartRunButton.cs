using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartRunButton : MonoBehaviour
{
    public void OnTap()
    {
        GameManager.gm.StartNewRun();
    }
}
