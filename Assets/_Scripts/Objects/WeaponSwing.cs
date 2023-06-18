using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    public float swaySmooth;
    public float swayMultiplier;
    public float swingForce = 1f;
    public float swingSpeed = 1f;
    public PlayerController playerController;

    private Quaternion initialRotation;
    private Vector3 rotationOffset;

    private void Start()
    {
        initialRotation = transform.localRotation;
        rotationOffset = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        Sway();
        Swing();
    }
    private void Swing()
    {
        if (playerController != null)
        {
            if (playerController.isMoving)
            {
                float swingAmount = Mathf.Sin(Time.time * swingSpeed) * swingForce;

                Quaternion swingRotation = Quaternion.Euler(rotationOffset.x + swingAmount, rotationOffset.y + swingAmount, rotationOffset.z);

                transform.localRotation = initialRotation * swingRotation;
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, Time.deltaTime * swingSpeed);
            }
        }
    }

    private void Sway()
    {
        float inputX = Input.GetAxis("Mouse X") * swayMultiplier;
        float inputY = Input.GetAxis("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-inputY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(inputX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmooth * Time.deltaTime);
    }
}