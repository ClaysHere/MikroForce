using UnityEngine;

public class StartupSettings : MonoBehaviour
{
    void Awake()
    {
        // Forces resolution to 1920x1080 and windowed (change to true for fullscreen)
        Screen.SetResolution(1920, 1080, false);
    }
}


