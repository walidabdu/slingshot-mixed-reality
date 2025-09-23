using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class BallSwooshSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip swooshSound;        // Assign your swoosh clip in the Inspector
    public float velocityThreshold = 5f; // Minimum speed before swoosh plays

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool hasPlayedSound = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = swooshSound;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        float speed = rb.velocity.magnitude;

        if (speed >= velocityThreshold && !hasPlayedSound)
        {
            audioSource.PlayOneShot(swooshSound);
            hasPlayedSound = true;
        }

        // Reset when speed drops (e.g., after bounce or rest)
        if (speed < 1f)
        {
            hasPlayedSound = false;
        }
    }
}
