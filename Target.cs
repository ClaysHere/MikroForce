using UnityEngine;

public class Target : MonoBehaviour
{
    // Ini adalah kesehatan dasar yang bisa diatur di Inspector.
    // Kita akan menggunakan ini sebagai dasar sebelum penyesuaian level.
    [Header("Base Health")]
    public float baseHealth = 50f;

    // Ini adalah modifier kesehatan berdasarkan level.
    // Anda bisa mengaturnya di Inspector untuk setiap level.
    [Header("Health Modifiers per Level")]
    public float level1HealthModifier = 0f;    // Contoh: Tidak ada perubahan untuk Level 1
    public float level2HealthModifier = 25f;   // Contoh: Tambah 50 health untuk Level 2
    public float level3HealthModifier = 50f;  // Contoh: Tambah 100 health untuk Level 3

    // Kesehatan aktual yang akan digunakan dalam game
    private float currentHealth;

    // Properti publik untuk membaca kesehatan saat ini dari luar (opsional)
    public float CurrentHealth { get { return currentHealth; } }


    void Start()
    {
        // Dapatkan level saat ini dari GameManager
        int currentLevel = GameManager.currentLevel;

        // Hitung kesehatan awal berdasarkan level
        float finalHealth = baseHealth;
        switch (currentLevel)
        {
            case 1:
                finalHealth += level1HealthModifier;
                break;
            case 2:
                finalHealth += level2HealthModifier;
                break;
            case 3:
                finalHealth += level3HealthModifier;
                break;
            default:
                Debug.LogWarning("Target: currentLevel was not set or invalid, using base health only.");
                break;
        }

        currentHealth = finalHealth;
        Debug.Log(gameObject.name + " initialized with " + currentHealth + " health for Level " + currentLevel);
    }

    // Metode yang dipanggil untuk memberikan kerusakan
    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Kurangi kesehatan
        Debug.Log(gameObject.name + " took " + amount + " damage. Current Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die(); // Jika kesehatan nol atau kurang, musuh mati
        }
    }

    // Metode yang dipanggil saat musuh mati
    void Die()
    {
        Debug.Log(gameObject.name + " died!");
        // Di sini Anda bisa menambahkan efek kematian (animasi, suara, partikel)
        Destroy(gameObject); // Hancurkan GameObject musuh
    }
}