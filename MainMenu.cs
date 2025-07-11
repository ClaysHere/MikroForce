#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        Cursor.visible = true;
    }

    public void Instructions()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Quit()
    {
        if (Application.isEditor)
        {
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false; // Stop play mode in the editor
            #endif
        }
        else
        {
            Application.Quit(); // Quits the game in the built version
        }
    }
}
