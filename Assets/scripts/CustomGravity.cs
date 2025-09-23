using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    // Start is called before the first frame update
   public float gravityMultiplier = 2f;  // Gravity scale factor
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply custom gravity
        Vector3 customGravity = Physics.gravity * gravityMultiplier;
        rb.AddForce(customGravity, ForceMode.Acceleration);
    }
}
