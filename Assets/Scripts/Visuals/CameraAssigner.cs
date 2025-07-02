using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAssigner : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Canvas>().worldCamera = GameManager.instance.mainCamera;
    }
}
