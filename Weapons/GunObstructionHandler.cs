using UnityEngine;

public class GunObstructionHandler : MonoBehaviour
{
    public Transform gunObject;              // e.g. M9
    public GameObject crosshair;             // e.g. Crosshair
    public GameObject disabledCrosshair;     // e.g. DisabledCrosshair
    public float hideDistance = 0.5f;
    public LayerMask wallMask;

    private bool gunHidden = false;

    private void Start()
    {
        SetCrosshairVisibility(true);
    }
    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, hideDistance, wallMask))
        {
            if (!gunHidden)
            {
                gunObject.gameObject.SetActive(false);
                SetCrosshairVisibility(false);
                gunHidden = true;
            }
        }
        else
        {
            if (gunHidden)
            {
                gunObject.gameObject.SetActive(true);
                SetCrosshairVisibility(true);
                gunHidden = false;
            }
        }
    }

    void SetCrosshairVisibility(bool isGunActive)
    {
        if (crosshair != null) crosshair.SetActive(isGunActive);
        if (disabledCrosshair != null) disabledCrosshair.SetActive(!isGunActive);
    }
}
