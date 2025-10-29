using UnityEngine;
using TMPro;

public class PlayAudioAndScoreOnCollision : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip collisionSound;
    private AudioSource audioSource;

    [Header("Score Settings")]
    public int scoreForOne = 10;
    public int scoreForTwo = 20;
    public int scoreForThree = 30;
    public int ScoreForCube = 20;

    // Track how many times THIS object has been hit
    private int localHitCount = 0;

    [Header("Optional: show local hit count")]
    public TextMeshPro localHitText; // optional 3D TMP to show per-object hit count

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collisionSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("ball")) return;

        int pointsToAdd = 0;

        if (gameObject.CompareTag("Ethio Telecome"))
            pointsToAdd = scoreForOne;
        else if (gameObject.CompareTag("Telebirr"))
            pointsToAdd = scoreForTwo;
        else if (gameObject.CompareTag("Zemen Gebeya"))
            pointsToAdd = scoreForThree;
        else if (gameObject.CompareTag("cube"))
            pointsToAdd = ScoreForCube;

        if (pointsToAdd > 0)
        {
            // Play sound each hit
            if (audioSource != null && collisionSound != null)
                audioSource.PlayOneShot(collisionSound);

            // Increment local hit count and update optional local display
            localHitCount++;
            if (localHitText != null)
            {
                localHitText.text = "x" + localHitCount.ToString();
            }

            // Register with ScoreManager including points
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.RegisterHit(gameObject.tag, pointsToAdd);
            }
        }
    }
}
