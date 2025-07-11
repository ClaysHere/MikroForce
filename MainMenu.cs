#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // Diperlukan untuk List

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("MainMenu Start: Cursor visible and unlocked.");
    }

    public void Instructions()
    {
        Debug.Log("Instructions button pressed. Loading Scene 2.");
        SceneManager.LoadSceneAsync(2);
    }

    public void QuitGameAndCleanup()
    {
        Debug.Log("Quit button pressed. Initiating cleanup and exit.");

        // === 3. Keluar dari Aplikasi ===
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            Debug.Log("Quit: Stopping play mode in editor.");
            EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            Debug.Log("Quit: Calling Application.Quit().");
            Application.Quit();

            // Log tambahan jika App.Quit() gagal secara internal
            Debug.LogError("Application.Quit() call did not result in immediate application termination. Check logs for issues.");
        }
    }
}