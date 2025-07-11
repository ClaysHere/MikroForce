using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true; // Pastikan kursor terlihat di menu
        Cursor.lockState = CursorLockMode.None; // Bebaskan kursor
    }

    public void Level1()
    {
        GameManager.currentLevel = 1; // Set level ke 1
        SceneManager.LoadSceneAsync(3); // Muat scene InGame (asumsi index 3)
    }

    public void Level2()
    {
        GameManager.currentLevel = 2; // Set level ke 2
        PlayGame();
    }

    public void Level3()
    {
        GameManager.currentLevel = 3; // Set level ke 3
        PlayGame();
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(4); // Muat scene InGame (asumsi index 3)
    }

    // Fungsi untuk kembali ke Main Menu (misal scene index 0)
    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}