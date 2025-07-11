using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class InGame : MonoBehaviour
{
    private bool isSwitching = false;
    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI timerText;

    // Tambahan untuk Pause Menu
    [Header("Pause Menu")]
    public GameObject pauseCanvas; // Referensi ke GameObject Canvas pause menu
    public bool isPaused = false; // Status pause game

    // Parameter game yang bisa disesuaikan per level
    [Header("Level Parameters")]
    public float baseTimeLimit = 60f;
    public float level1TimeBonus = 0f;
    public float level2TimeBonus = -15f;
    public float level3TimeBonus = -30f;

    private float timeLimit;
    private float timeRemaining;
    private bool gameEnded = false;
    private int initialEnemyCount = 0;

    void Start()
    {
        // === PENTING: Penanganan kursor di awal scene InGame ===
        // Sembunyikan dan kunci kursor saat game dimulai (kondisi normal InGame)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Pastikan pause canvas tidak aktif di awal
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }

        int currentLevel = GameManager.currentLevel;
        Debug.Log("InGame scene loaded. Current Level: " + currentLevel);

        GameObject[] enemiesNormal = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemiesHard = GameObject.FindGameObjectsWithTag("Hard_Enemy");

        if (currentLevel == 1)
        {
            foreach (GameObject enemy in enemiesHard) { enemy.SetActive(false); ToggleOutline(enemy, false); }
            initialEnemyCount = enemiesNormal.Length;
            foreach (GameObject enemy in enemiesNormal) { ToggleOutline(enemy, true); }
        }
        else if (currentLevel == 2)
        {
            foreach (GameObject enemy in enemiesHard) { enemy.SetActive(false); ToggleOutline(enemy, false); }
            initialEnemyCount = enemiesNormal.Length;
            foreach (GameObject enemy in enemiesNormal) { ToggleOutline(enemy, false); }
        }
        else if (currentLevel == 3)
        {
            foreach (GameObject enemy in enemiesNormal) { enemy.SetActive(false); ToggleOutline(enemy, false); }
            initialEnemyCount = enemiesHard.Length;
            foreach (GameObject enemy in enemiesHard) { ToggleOutline(enemy, false); }
        }
        else
        {
            Debug.LogWarning("InGame: currentLevel not set or invalid. All enemies will be visible and their outline state depends on Inspector settings.");
            initialEnemyCount = enemiesNormal.Length + enemiesHard.Length;
        }

        float calculatedTimeLimit = baseTimeLimit;
        switch (currentLevel)
        {
            case 1: calculatedTimeLimit += level1TimeBonus; break;
            case 2: calculatedTimeLimit += level2TimeBonus; break;
            case 3: calculatedTimeLimit += level3TimeBonus; break;
            default: Debug.LogWarning("InGame: currentLevel not set or invalid, using base time limit."); break;
        }
        timeLimit = calculatedTimeLimit;
        timeRemaining = timeLimit;

        if (initialEnemyCount == 0)
        {
            Debug.LogWarning("No enemies found with relevant tags for this level! Ensure enemies have the correct tags and 'Target' script.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                ResumeGame();
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                PauseGame();
            }
        }

        if (gameEnded || isPaused) return;

        timeRemaining -= Time.deltaTime;
        int secondsRemaining = Mathf.CeilToInt(timeRemaining);

        if (secondsRemaining <= 10)
        {
            timerText.text = "Time Left: <color=red>" + secondsRemaining.ToString() + "s</color>";
        }
        else
        {
            timerText.text = "Time Left: " + secondsRemaining.ToString() + "s";
        }

        int enemiesAlive = 0;
        int currentLevel = GameManager.currentLevel;

        if (currentLevel == 1 || currentLevel == 2)
        {
            enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        else if (currentLevel == 3)
        {
            enemiesAlive = GameObject.FindGameObjectsWithTag("Hard_Enemy").Length;
        }
        else
        {
            enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("Hard_Enemy").Length;
        }

        enemyText.text = "Enemies Killed : " + (initialEnemyCount - enemiesAlive) + "/" + initialEnemyCount;

        if (!isSwitching && enemiesAlive == 0)
        {
            StartCoroutine(LoadNextSceneWithDelay(2f, "Win"));
        }

        if (timeRemaining <= 0 && enemiesAlive > 0)
        {
            StartCoroutine(LoadNextSceneWithDelay(2f, "GameOver"));
        }
    }

    private void ToggleOutline(GameObject obj, bool enable)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = enable;
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        // === PENTING: Kursor saat di-pause ===
        Cursor.visible = true; // Tampilkan kursor
        Cursor.lockState = CursorLockMode.None; // Bebaskan kursor
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        // === PENTING: Kursor saat dilanjutkan (kembali ke kondisi InGame normal) ===
        Cursor.visible = false; // Sembunyikan kursor
        Cursor.lockState = CursorLockMode.Locked; // Kunci kursor
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
    }

    public void GoToMainMenu()
    {
        ResumeGame(); // Pastikan waktu kembali normal dan kursor dikembalikan ke keadaan InGame normal sebelum pindah scene
        Debug.Log("Going to Main Menu (Scene Index 1)...");
        // === PENTING: Kursor saat pindah ke scene lain (misalnya Main Menu) ===
        Cursor.visible = true; // Pastikan kursor terlihat
        Cursor.lockState = CursorLockMode.None; // Pastikan kursor bebas
        SceneManager.LoadScene(1); // Muat scene dengan index 1
    }

    IEnumerator LoadNextSceneWithDelay(float delay, string result)
    {
        isSwitching = true;
        gameEnded = true;
        Time.timeScale = 1f;
        // === PENTING: Kursor saat pindah scene dari Win/GameOver ===
        Cursor.lockState = CursorLockMode.None; // Bebaskan kursor
        Cursor.visible = true; // Tampilkan kursor
        yield return new WaitForSeconds(delay);

        if (result == "Win")
        {
            Debug.Log("You Win! Loading Main Menu...");
            GameManager.result = "Win";
            SceneManager.LoadSceneAsync(6);
        }
        else
        {
            Debug.Log("Game Over! Loading Game Over Screen...");
            GameManager.result = "Lose";
            SceneManager.LoadSceneAsync(5);
        }
    }
}