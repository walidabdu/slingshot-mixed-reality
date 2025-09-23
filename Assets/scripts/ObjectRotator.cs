using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("Objects to Rotate")]
    public GameObject[] objectsToRotate;   // Add objects in Inspector

    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up; // Axis of rotation (default: Y-axis)
    public float rotationSpeed = 50f;         // Speed in degrees per second

    void Update()
    {
        foreach (GameObject obj in objectsToRotate)
        {
            if (obj != null)
            {
                obj.transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
