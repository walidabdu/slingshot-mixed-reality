using UnityEngine;

public class CubeKnockbackDetector : MonoBehaviour
{
    [Header("Knockback Detection Settings")]
    [Tooltip("Minimum movement speed to count as affected (knocked).")]
    public float velocityThreshold = 1.0f;

    [Tooltip("Optional: Distance from original position to also count as affected.")]
    public float displacementThreshold = 0.8f;

    [Tooltip("Score given when cube is affected indirectly.")]
    public int indirectHitScore = 10;

    private Rigidbody rb;
    private Vector3 originalPosition;
    private bool hasScored = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
    }

    void Update()
    {
        if (hasScored) return;

        // Detect significant movement or displacement
        bool movedFast = rb != null && rb.velocity.magnitude > velocityThreshold;
        bool displaced = Vector3.Distance(transform.position, originalPosition) > displacementThreshold;

        if (movedFast || displaced)
        {
            hasScored = true;

            // Register with existing ScoreManager (no changes needed)
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.RegisterHit("cube", indirectHitScore);
            }

            Debug.Log($"{gameObject.name} was affected by impact! +{indirectHitScore} points");
        }
    }
}
