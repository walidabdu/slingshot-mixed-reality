using UnityEngine;

public class UpDownMover : MonoBehaviour
{
    [Header("Objects to Move")]
    public GameObject[] objectsToMove;   // Add objects in the Inspector

    [Header("Movement Settings")]
    public float moveDistance = 2f;      // How far they move up & down
    public float moveSpeed = 2f;         // Speed of movement

    // Internal data to track per-object state
    private Vector3[] startPositions;
    // Per-object motion parameters for independent movement
    private float[] amplitudes;
    private float[] frequencies;
    private float[] phases;

    void Start()
    {
        // Initialize arrays
        int n = objectsToMove.Length;
        startPositions = new Vector3[n];
        amplitudes = new float[n];
        frequencies = new float[n];
        phases = new float[n];

        for (int i = 0; i < n; i++)
        {
            GameObject obj = objectsToMove[i];
            if (obj == null) continue;

            startPositions[i] = obj.transform.position;

            // Give each object a slightly random amplitude and frequency so they don't sync
            amplitudes[i] = Random.Range(0.5f * moveDistance, 1.5f * moveDistance);
            frequencies[i] = Random.Range(0.5f * moveSpeed, 1.5f * moveSpeed);
            phases[i] = Random.Range(0f, Mathf.PI * 2f);
        }
    }

    void Update()
    {
        float t = Time.time;
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            GameObject obj = objectsToMove[i];
            if (obj == null) continue;

            // Smooth independent up/down using a sine wave per object
            float yOffset = Mathf.Sin(t * frequencies[i] + phases[i]) * amplitudes[i];
            Vector3 targetPos = startPositions[i] + new Vector3(0f, yOffset, 0f);
            obj.transform.position = targetPos;
        }
    }
}
