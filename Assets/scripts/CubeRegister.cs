using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeRegister : MonoBehaviour
{
    [Header("Scoring Settings")]
    public int directHitScore = 20;       // Score for direct hit
    public int fallScore = 10;            // Score for cubes that fell
    public float fallDistance = 0.5f;     // How far it must move to count as fallen

    [Header("Audio")]
    public AudioSource hitSound;          // Optional audio for when hit

    private Vector3 startPos;
    private bool hasGivenFallScore = false;
    private bool hasGivenHitScore = false;
    private Rigidbody rb;

    private void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();

        // Register this cube to the manager (for resetting later)
        if (CubeManager.Instance != null)
            CubeManager.Instance.RegisterCube(transform);
    }

    private void Update()
    {
        // Check if cube has fallen/moved significantly
        if (!hasGivenFallScore && Vector3.Distance(transform.position, startPos) > fallDistance)
        {
            hasGivenFallScore = true;
           // ScoreManager.Instance.AddScore(fallScore);
            ScoreManager.Instance.RegisterHit(gameObject.tag, fallScore);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball") && !hasGivenHitScore)
        {
            hasGivenHitScore = true;
            ScoreManager.Instance.AddScore(directHitScore);

            if (hitSound != null)
                hitSound.Play();

            // Trigger global reset after delay
            if (CubeManager.Instance != null)
                CubeManager.Instance.ResetAllCubes();
        }
    }

    // Called by CubeManager when resetting
    public void ResetState()
    {
        hasGivenFallScore = false;
        hasGivenHitScore = false;
    }
}
