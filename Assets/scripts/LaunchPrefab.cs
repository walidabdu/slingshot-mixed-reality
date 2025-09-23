using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LaunchPrefab : MonoBehaviour
{
    private Rigidbody objectRb;

    [Header("Launch Settings")]
    public float baseForce = 2.5f;
    public float forceVariation = 0.05f;
    public bool normalizeByScale = false; // Toggle to adjust for prefab scale

    [Header("Custom Gravity")]
    public float customGravity = -1.5f;

    [Header("Torque Settings")]
    public float maxTorque = 0.1f;

    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        objectRb.useGravity = false;

        float launchForce = ComputeLaunchForce();
        objectRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

        objectRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        Debug.Log($"{gameObject.name} | Scale: {transform.localScale} | LaunchForce: {launchForce}");
    }

    void FixedUpdate()
    {
        objectRb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
    }

    float ComputeLaunchForce()
    {
        float rawForce = baseForce + Random.Range(-forceVariation, forceVariation);

        if (!normalizeByScale)
            return rawForce;

        float scaleMagnitude = transform.localScale.magnitude;
        float scaleFactor = Mathf.Clamp(1f / scaleMagnitude, 0.1f, 2f); // prevent overcompensation
        return rawForce * scaleFactor;
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }
}
