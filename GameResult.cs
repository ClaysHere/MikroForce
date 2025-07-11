using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResult : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true; // Pastikan kursor terlihat di menu
        Cursor.lockState = CursorLockMode.None; // Bebaskan kursor

        if (GameManager.result is "Win")
        {
            Debug.Log(GameManager.result);
            GameObject stayTuned = GameObject.FindGameObjectWithTag("Stay_Tuned");
            GameObject continueButton = GameObject.FindGameObjectWithTag("Continue");

            if (GameManager.currentLevel == 3)
            {
                Debug.Log("Stay Tuned for next level"); 
                if (stayTuned == null)
                    Debug.LogError("Continue button not found!");
                else
                    stayTuned.SetActive(true);
                    continueButton.SetActive(false);
            }
            else
            {
                Debug.Log("Continue Next Level");
                if (continueButton == null)
                    Debug.LogError("Continue button not found!");
                else
                    stayTuned.SetActive(false);
                    continueButton.SetActive(true);
            }
        }
    }
    public void Continue()
    {
        GameManager.currentLevel += 1;
        SceneManager.LoadSceneAsync(4);
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Retry()
    {
        SceneManager.LoadSceneAsync(4);
    }
}
