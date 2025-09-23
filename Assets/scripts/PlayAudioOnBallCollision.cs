using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnBallCollision : MonoBehaviour
{
    public AudioClip collisionSound;  // Assign the audio clip in the Inspector
    private AudioSource audioSource;

    private bool hasPlayed = false;   // To ensure the audio plays only once

    // Start is called before the first frame update
    void Start()
    {
        // Add an AudioSource component to the GameObject and configure it
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collisionSound;
    }

    // This method is called when the GameObject's collider collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has the tag "ball"
        if (collision.gameObject.CompareTag("ball") && !hasPlayed)
        {
            // Play the audio clip once
            audioSource.Play();
            hasPlayed = true;  // Set flag to prevent multiple plays
        }
    }
}
