using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; // Diperlukan untuk metode Contains()

public class GameManager : MonoBehaviour
{
    public static int currentLevel = 0;
    public static string result = "";
    public static GameManager Instance;

    public bool playMusic = false; // Ini tampaknya tidak digunakan di HandleMusic, bisa dihapus atau diintegrasikan
    public AudioSource lobbyMusic;
    public int[] pauseMusicInSceneIndexes;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Jika lobbyMusic belum di-assign di Inspector, coba cari di objek ini
        if (lobbyMusic == null)
        {
            lobbyMusic = GetComponent<AudioSource>();
        }

        // Pastikan lobbyMusic tidak null sebelum mengaturnya
        if (lobbyMusic != null)
        {
            lobbyMusic.loop = true; // Asumsi Anda ingin musik ini looping
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // Panggil HandleMusic hanya jika ini adalah instance utama dan bukan duplikat yang dihancurkan
        if (Instance == this)
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            HandleMusic(currentScene);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hanya panggil HandleMusic jika ini adalah instance utama
        if (Instance == this)
        {
            HandleMusic(scene.buildIndex);
        }
    }

    void HandleMusic(int sceneIndex)
    {
        if (lobbyMusic == null)
        {
            Debug.LogWarning("GameManager: lobbyMusic AudioSource is not assigned!");
            return;
        }

        // Menggunakan LINQ untuk pencarian yang lebih bersih
        bool shouldPause = pauseMusicInSceneIndexes.Contains(sceneIndex);

        if (shouldPause)
        {
            if (lobbyMusic.isPlaying)
            {
                lobbyMusic.Pause();
                Debug.Log("Lobby music paused in scene " + sceneIndex);
            }
        }
        else
        {
            // Hanya putar jika tidak sedang diputar dan game tidak sedang di-pause
            // Asumsi Time.timeScale tidak 0 saat ini
            if (!lobbyMusic.isPlaying && Time.timeScale > 0)
            {
                lobbyMusic.Play();
                Debug.Log("Lobby music played in scene " + sceneIndex);
            }
        }
    }

    void OnDestroy()
    {
        // Pastikan listener dilepas jika objek dihancurkan dengan cara lain (misal, duplikat)
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("GameManager: OnDestroy called. SceneManager.sceneLoaded listener removed.");

        // Jika instance yang dihancurkan adalah instance utama, reset Instance
        if (Instance == this)
        {
            Instance = null;
        }
    }
}