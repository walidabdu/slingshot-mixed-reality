using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_On_Collision : MonoBehaviour
{
    private HashSet<int> collidedOnceIds = new HashSet<int>();

    private void OnCollisionEnter(Collision collision)
    {
        // Tags to check
        string[] destroyTags = { "Ethio Telecome", "Telebirr", "Zemen Gebeya" };

        // If the collided object's tag is in the list, destroy it
        foreach (string tag in destroyTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                int id = collision.gameObject.GetInstanceID();

                if (collidedOnceIds.Contains(id))
                {
                    // Second collision: destroy immediately
                    Destroy(collision.gameObject, 0f);
                }
                else
                {
                    // First time: record and destroy after delay
                    collidedOnceIds.Add(id);
                    Destroy(collision.gameObject, 4f);
                }

                break; // Exit loop once matched
            }
        }
    }
}

