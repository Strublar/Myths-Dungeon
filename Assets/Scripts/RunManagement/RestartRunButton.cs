using UnityEngine;

public class RestartRunButton : MonoBehaviour
{
    public void OnTap()
    {
        MenuManager.instance.OnStartNewRun();
    }
}
