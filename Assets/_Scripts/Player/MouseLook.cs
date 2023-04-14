using UnityEngine;

public class MouseLook : MonoBehaviour {
    public Transform target;
    public float smoothSpeed = 0.125f; 
    public Vector3 offset; 

    public float mouseSensitivity = 100f;
    public Transform playerBody; 
    float xRotation = 0f; 
    float yRotation = 0f;

    void LateUpdate() {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); 
        transform.position = smoothedPosition; 

        transform.LookAt(target); 

       
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
