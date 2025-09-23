using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnBallAndChipsCollision : MonoBehaviour
{
    public AudioClip collisionSound;  // Assign the audio clip in the Inspector
    private AudioSource audioSource;

    private bool hasPlayed = false;   // To ensure the audio plays only once

    void Start()
    {
        // Add an AudioSource component to the GameObject and configure it
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collisionSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if this GameObject has tag "Ethio Telecome","two","3" and collided with "ball"
        if ((gameObject.CompareTag("Ethio Telecome") || gameObject.CompareTag("Telebirr") || gameObject.CompareTag("Zemen Gebeya")) 
            && collision.gameObject.CompareTag("ball") 
            && !hasPlayed)
        {
            // Play the audio clip once
            audioSource.Play();
            hasPlayed = true;  // Prevent multiple plays
        }
    }
}
