using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float damage = 25f; // Damage yang diberikan per tembakan
    public float range = 100f; // Jarak tembak maksimum
    public float impactForce = 30f; // Kekuatan dorong ke objek yang kena (jika punya Rigidbody)
    public float fireRate = 2f; // Kecepatan tembak (tembakan per detik)

    [Header("References")]
    public Camera fpsCam; // Kamera FPS pemain (biasanya main camera)
    public AudioSource gunAudio; // AudioSource untuk suara tembakan

    // Referensi untuk efek visual tembakan
    private ParticleSystem[] muzzleFlashParticles; // Array karena bisa ada beberapa ParticleSystem anak
    private Light muzzleFlashLight; // Cahaya muzzle flash
    private Animator handgunAnimator; // Animator untuk animasi handgun
    private Animator cameraAnimator; // Animator untuk animasi kamera (recoil)

    public GameObject hitMark;
    private Coroutine hitEffectCoroutine;

    private float nextTimeToFire = 0f; // Kapan tembakan berikutnya bisa dilakukan

    void Start()
    {
        hitMark.SetActive(false);
        // Dapatkan semua ParticleSystem anak untuk muzzle flash
        muzzleFlashParticles = GetComponentsInChildren<ParticleSystem>();

        // Dapatkan Light anak untuk muzzle flash
        muzzleFlashLight = GetComponentInChildren<Light>();
        if (muzzleFlashLight != null)
            muzzleFlashLight.enabled = false; // Matikan cahaya di awal

        // Dapatkan animator dari senjata (asumsi di parent atau di objek yang sama)
        // Atau Anda bisa drag & drop manual di Inspector jika animator di GameObject lain
        handgunAnimator = GetComponent<Animator>();
        if (handgunAnimator == null && transform.parent != null)
            handgunAnimator = transform.parent.GetComponent<Animator>(); // Coba di parent jika tidak ditemukan di objek ini

        // Dapatkan animator kamera (asumsi di fpsCam atau parent-nya)
        if (fpsCam != null)
        {
            cameraAnimator = fpsCam.GetComponent<Animator>();
            if (cameraAnimator == null && fpsCam.transform.parent != null)
                cameraAnimator = fpsCam.transform.parent.GetComponent<Animator>();
        }

        // Cek jika ada referensi yang hilang
        if (fpsCam == null) Debug.LogWarning("Gun: fpsCam not assigned! Raycast might not work correctly.");
        if (gunAudio == null) Debug.LogWarning("Gun: gunAudio not assigned! No gun sound.");
    }

    void Update()
    {
        // Deteksi input tembak (klik kiri mouse) dan cek fire rate
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) // Menggunakan GetButton agar bisa terus menembak saat ditahan
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        // Jika ingin tembak sekali klik:
        // if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        // {
        //     nextTimeToFire = Time.time + 1f / fireRate;
        //     Shoot();
        // }
    }

    void Shoot()
    {
        // Mainkan suara tembakan
        if (gunAudio != null)
            gunAudio.Play();

        // Mainkan efek partikel muzzle flash
        if (muzzleFlashParticles != null)
        {
            foreach (ParticleSystem ps in muzzleFlashParticles)
            {
                ps.Play();
            }
        }

        // Aktifkan dan matikan cahaya muzzle flash
        if (muzzleFlashLight != null)
            StartCoroutine(FlashMuzzleLight());

        // Mulai animasi recoil
        StartCoroutine(Recoil());

        // Lakukan Raycast dari kamera FPS
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // Coba dapatkan komponen Target dari objek yang terkena
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                // Jika ditemukan komponen Target, panggil TakeDamage
                if (hitEffectCoroutine != null)
                {
                    StopCoroutine(hitEffectCoroutine);
                }
                hitEffectCoroutine = StartCoroutine(ShowHitMarker(0.1f));
                target.TakeDamage(damage);
            }

            // Berikan gaya dorong (impact force) jika objek memiliki Rigidbody
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }

    // Coroutine untuk efek cahaya muzzle flash
    IEnumerator FlashMuzzleLight()
    {
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.enabled = true;
            yield return new WaitForSeconds(0.05f); // Durasi cahaya flash sangat singkat
            muzzleFlashLight.enabled = false;
        }
    }

    // Coroutine untuk efek recoil (animasi senjata dan kamera)
    IEnumerator Recoil()
    {
        if (handgunAnimator != null) handgunAnimator.Play("HandgunFire");
        if (cameraAnimator != null) cameraAnimator.Play("HandgunCamera");

        yield return new WaitForSeconds(0.4f); // Durasi animasi tembak

        if (handgunAnimator != null) handgunAnimator.Play("New State"); // Kembali ke state idle (ganti "New State" dengan nama state idle Anda)
        if (cameraAnimator != null) cameraAnimator.Play("New State"); // Kembali ke state idle kamera

        yield return new WaitForSeconds(0.1f); // Sedikit jeda tambahan
    }

    IEnumerator ShowHitMarker(float duration)
    {
        if (hitMark != null) hitMark.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (hitMark != null) hitMark.SetActive(false);
    }
}