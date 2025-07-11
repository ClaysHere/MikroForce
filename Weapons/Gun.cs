using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 1f;


    public Camera fpsCam;
    public AudioSource gunAudio;
    private ParticleSystem[] muzzleFlash;
    private Light muzzleFlashlight;
    private Animator recoil;
    [SerializeField] GameObject handgun;
    [SerializeField] GameObject mainCamera;

    private float nextTimeToFire = 0f;
    private bool flash;

    void Start()
    {
        muzzleFlash = GetComponentsInChildren<ParticleSystem>();
        muzzleFlashlight = GetComponentInChildren<Light>();
        if (muzzleFlashlight != null)
            muzzleFlashlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        gunAudio.Play();
        foreach (ParticleSystem ps in muzzleFlash)
        {
            ps.Play();
        }
        if (muzzleFlashlight != null)
            StartCoroutine(FlashMuzzleLight());

        StartCoroutine(Recoil());

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
        
    }
    IEnumerator FlashMuzzleLight()
    {
        muzzleFlashlight.enabled = true;
        yield return new WaitForSeconds(0.2f); // Flash duration
        muzzleFlashlight.enabled = false;
    }

    IEnumerator Recoil()
    {
        handgun.GetComponent<Animator>().Play("HandgunFire");
        mainCamera.GetComponent<Animator>().Play("HandgunCamera");
        yield return new WaitForSeconds(0.4f);
        handgun.GetComponent<Animator>().Play("New State");
        mainCamera.GetComponent<Animator>().Play("New State");
        yield return new WaitForSeconds(0.1f);
    }
}
