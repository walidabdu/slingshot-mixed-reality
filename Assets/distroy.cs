using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distroy : MonoBehaviour
{
    // Start is called before the first frame update
    // This method is called when the GameObject's collider enters a trigger or collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has the tag "chips"
        if (collision.gameObject.CompareTag("chips"))
        {
            // Destroy the object with tag "chips"
            Destroy(collision.gameObject);
        }
    }
}
