using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    private bool isSwitching = false;
    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI timerText;
    public GameObject[] enemies;

    public float timeLimit = 15f; // misal 60 detik
    private float timeRemaining;
    private bool gameEnded = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        timeRemaining = timeLimit;
    }

    void Update()
    {
        if (gameEnded) return;

        // Hitung mundur waktu
        timeRemaining -= Time.deltaTime;
        int secondsRemaining = Mathf.CeilToInt(timeRemaining);

        // Change timer text color to red if 10 seconds or under
        if (secondsRemaining <= 10)
        {
            timerText.text = "Time Left: <color=red>" + secondsRemaining.ToString() + "s</color>";
        }
        else
        {
            timerText.text = "Time Left: " + secondsRemaining.ToString() + "s";
        }

        // Update jumlah musuh yang tersisa
        GameObject[] remaining = GameObject.FindGameObjectsWithTag("Enemy");
        enemyText.text = "Enemies Killed : " + (enemies.Length - remaining.Length) + "/" + enemies.Length;

        // Cek kondisi menang
        if (!isSwitching && remaining.Length == 0)
        {
            StartCoroutine(LoadNextSceneWithDelay(2f, "Win"));
        }

        // Cek kondisi kalah
        if (timeRemaining <= 0 && remaining.Length > 0)
        {
            StartCoroutine(LoadNextSceneWithDelay(2f, "GameOver"));
        }
    }

    IEnumerator LoadNextSceneWithDelay(float delay, string result)
    {
        isSwitching = true;
        gameEnded = true;
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(delay);

        // Tampilkan hasil menang/kalah
        if (result == "Win")
        {
            Debug.Log("You Win!");
            SceneManager.LoadSceneAsync(0); // change '0' to desired scene 
        }
        else
        {
            Debug.Log("Game Over!");
            SceneManager.LoadSceneAsync(2); // change '0' to desired scene 
        }

        //if (result == "Win")
        //    SceneManager.LoadScene("WinScene"); // Ganti dengan nama scene kemenangan
        //else
        //    SceneManager.LoadScene("GameOverScene"); // Ganti dengan nama scene Game Over
    }
}