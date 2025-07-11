using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; // Diperlukan untuk metode Contains()

[RequireComponent(typeof(AudioSource))]
public class LobbyMusic : MonoBehaviour
{
    public int[] pauseMusicInSceneIndexes; // e.g. [0] → musik tidak diputar di Splash

    private static LobbyMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        Debug.Log("LobbyMusic: Awake. Current scene index: " + SceneManager.GetActiveScene().buildIndex);

        if (instance != null)
        {
            // Jika sudah ada instance, hancurkan yang baru ini
            Destroy(gameObject);
            Debug.Log("LobbyMusic: Duplicate instance destroyed.");
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Membuat objek ini persistent antar scene
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        // Langsung evaluasi scene saat ini saat pertama kali dibuat
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (ShouldPauseInScene(currentSceneIndex))
        {
            audioSource.Stop(); // Benar-benar tidak mulai jika seharusnya di-pause
            Debug.Log("LobbyMusic: Audio stopped in scene index: " + currentSceneIndex);
        }
        else
        {
            audioSource.Play();
            Debug.Log("LobbyMusic: Audio started in scene index: " + currentSceneIndex);
        }

        // Daftarkan ke event sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LobbyMusic: OnSceneLoaded. Scene index: " + scene.buildIndex);

        if (ShouldPauseInScene(scene.buildIndex))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                Debug.Log("LobbyMusic: Music paused in scene index: " + scene.buildIndex);
            }
        }
        else
        {
            // Hanya putar jika belum diputar dan game tidak sedang di-pause
            // Asumsi Time.timeScale tidak 0 saat ini
            if (!audioSource.isPlaying && Time.timeScale > 0)
            {
                audioSource.UnPause(); // Lanjutkan dari posisi terakhir
                Debug.Log("LobbyMusic: Music resumed in scene index: " + scene.buildIndex);
            }
        }
    }

    bool ShouldPauseInScene(int sceneIndex)
    {
        // Menggunakan LINQ (System.Linq) untuk pencarian yang lebih bersih
        return pauseMusicInSceneIndexes.Contains(sceneIndex);
    }

    void OnApplicationQuit()
    {
        // === PENTING: Membersihkan sumber daya saat aplikasi akan keluar ===
        Debug.Log("LobbyMusic: OnApplicationQuit called. Stopping audio and preparing to destroy.");
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Hentikan pemutaran audio
            Debug.Log("LobbyMusic: AudioSource stopped.");
        }

        // Hancurkan objek itu sendiri. Meskipun Unity akan membersihkan,
        // ini memastikan bahwa objek tidak "menggantung" lebih lama dari seharusnya.
        // Destroy(gameObject); // Ini opsional, karena Application.Quit() akan menghancurkan semuanya

        // Lepaskan event listener untuk menghindari potensi memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("LobbyMusic: SceneManager.sceneLoaded listener removed.");
    }

    void OnDestroy()
    {
        // Pastikan listener dilepas jika objek dihancurkan dengan cara lain (misal, duplicate)
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("LobbyMusic: OnDestroy called. SceneManager.sceneLoaded listener removed.");

        // Jika instance ini yang dihancurkan, reset instance
        if (instance == this)
        {
            instance = null;
        }
    }
}