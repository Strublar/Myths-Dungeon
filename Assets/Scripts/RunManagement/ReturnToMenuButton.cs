using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenuButton : MonoBehaviour
{
    public void OnTap()
    {
        RunManager.instance.ReturnToMenu();
    }
}
