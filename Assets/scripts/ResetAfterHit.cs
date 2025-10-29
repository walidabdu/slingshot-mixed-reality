using UnityEngine;
using System.Collections;

public class ResetAfterHit : MonoBehaviour
{
    [Header("Reset Settings")]
    [Tooltip("Time in seconds before starting to reset after being hit")]
    public float resetDelay = 3f;

    [Tooltip("How fast the object moves back to its original position")]
    public float resetSpeed = 2f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;
    private bool isResetting = false;

    void Start()
    {
        // Store the original position and rotation at start
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the hitting object has tag "ball"
        if (collision.gameObject.CompareTag("ball") && !isResetting)
        {
            StartCoroutine(ResetAfterDelay());
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        isResetting = true;

        // Wait the specified delay time before starting the reset
        yield return new WaitForSeconds(resetDelay);

        // Disable physics so the object can move back smoothly
        rb.isKinematic = true;

        float t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        // Smoothly move back to original position and rotation
        while (t < 1f)
        {
            t += Time.deltaTime * resetSpeed;
            transform.position = Vector3.Lerp(startPos, originalPosition, t);
            transform.rotation = Quaternion.Slerp(startRot, originalRotation, t);
            yield return null;
        }

        // Re-enable physics
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isResetting = false;
    }
}
