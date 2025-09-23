using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 6f;  // Time before the GameObject is destroyed (6 seconds by default)

    // Start is called before the first frame update
    void Start()
    {
        // Destroy the GameObject after the specified lifetime
        Destroy(gameObject, lifetime);
    }
}
