using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobSpeed = 14f;
    public float bobAmount = 0.05f;
    private float defaultYPos = 0;
    private float timer = 0;

    public bool isSprinting;

    private void Start()
    {
        defaultYPos = transform.localPosition.y;
    }

    private void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            timer += Time.deltaTime * (isSprinting ? bobSpeed * 1.5f : bobSpeed);
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultYPos, transform.localPosition.z);
        }
    }
}
