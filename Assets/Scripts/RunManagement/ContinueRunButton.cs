using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueRunButton : MonoBehaviour
{
    public void OnTap()
    {
        GameManager.gm.StartNewBoss();
    }
}
