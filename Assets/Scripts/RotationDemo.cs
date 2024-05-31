    using UnityEngine;
using UnityEngine.InputSystem;

public class RotationDemo : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public Transform pivotPoint;

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.qKey.wasPressedThisFrame)
        {
            // Quaternion Rotation
            Quaternion rotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
            transform.rotation = transform.rotation * rotation;
            Debug.Log("Quaternion Rotation");
        }

        if (keyboard.eKey.wasPressedThisFrame)
        {
            // Euler Angles Rotation
            transform.eulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);
            Debug.Log("Euler Angles Rotation");
        }

        if (keyboard.rKey.wasPressedThisFrame)
        {
            // Transform.Rotate
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            Debug.Log("Transform.Rotate");
        }

        if (keyboard.tKey.wasPressedThisFrame)
        {
            // Transform.RotateAround
            if (pivotPoint != null)
            {
                transform.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
                Debug.Log("Transform.RotateAround");
            }
            else
            {
                Debug.LogWarning("Pivot point not assigned for RotateAround");
            }
        }

        if (keyboard.yKey.wasPressedThisFrame)
        {
            // Set forward axis
            Vector3 newForward = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * transform.forward;
            transform.forward = newForward;
            Debug.Log("Set Forward Axis Rotation");
        }
    }
}