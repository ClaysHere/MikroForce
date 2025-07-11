using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Windows.WebCam.VideoCapture;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioSource))]
public class SplashScreen : MonoBehaviour
{
    public float zoomSpeed = 10f;         // Kecepatan zoom (FOV berkurang)
    public float targetFOV = 60f;         // Target FOV (semakin kecil, semakin zoom in)
    public float delayBeforeNextScene = 4f; // Lama splash sebelum pindah scene
    public float delayAudioAfterZoomStart = 2f;

    private Camera cam;
    private AudioSource audioSource;
    private bool audioStarted = false;

    void Start()
    {
        // Ambil komponen
        cam = GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();

        // Reset FOV awal
        cam.fieldOfView = 60f;

        // Mainkan audio splash
        if (audioSource != null)
        {
            // Delay untuk mulai audio (setengah jalan animasi zoom)
            Invoke(nameof(PlaySplashAudio), delayAudioAfterZoomStart);
        }

        // Jadwalkan pindah ke scene berikutnya
        Invoke("GoToMainMenu", delayBeforeNextScene);
    }

    void Update()
    {
        if (cam.fieldOfView > targetFOV)
        {
            cam.fieldOfView -= zoomSpeed * Time.deltaTime;
            cam.fieldOfView = Mathf.Max(cam.fieldOfView, targetFOV); // hindari lewat target
        }
    }

    void PlaySplashAudio()
    {
        if (audioSource != null && !audioStarted && audioSource.clip != null)
        {
            audioSource.Play();
            audioStarted = true;
            Debug.Log("Audio splash diputar.");
        }
    }
    void GoToMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene(1, LoadSceneMode.Single); // Ganti angka 1 dengan index atau nama scene tujuan
    }
}
