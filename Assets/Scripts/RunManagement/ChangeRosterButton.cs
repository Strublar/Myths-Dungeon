using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRosterButton : MonoBehaviour
{
    public void OnTap()
    {
        GameManager.gm.ChangeRoster();
    }
}
