using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructoncollision : MonoBehaviour
{
    public ParticleSystem collisionEffect;  // Assign the particle system prefab in the Inspector

    // This method is called when the GameObject's collider enters a trigger or collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has the tag "ball"
        if (collision.gameObject.CompareTag("ball"))
        {
            // Play the particle effect at the point of collision
            Instantiate(collisionEffect, transform.position, collisionEffect.transform.rotation);

            // Destroy this GameObject (the one this script is attached to)
            Destroy(gameObject,0.5f);
        }
    }
}
