using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float speed = 5f; // Character movement speed
    public float rotationSpeed = 360f; // Character rotation speed

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get player inputs
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement).normalized;

        // If there is any movement input, move the character
        if (movement.magnitude >= 0.1f)
        {
            // Calculate character rotation in a plane
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

            // Rotate towards the target direction
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the character in the input direction
            Vector3 movementVelocity = new Vector3(horizontalMovement, 0f, verticalMovement).normalized * speed;
            rb.MovePosition(rb.position + movementVelocity * Time.deltaTime);
        }
    }
}
