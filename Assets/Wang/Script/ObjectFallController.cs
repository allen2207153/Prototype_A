using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    public GameObject targetObject;             // The specified object to fall
    public Vector3 fallDirection = Vector3.forward; // Direction of the fall
    public float fallForce = 10f;               // Force of the fall
    public Animator playerAnimator;             // Animator component of the player

    private Rigidbody rb;

    void Start()
    {
        // Check and get Rigidbody of the target object
        if (targetObject != null)
        {
            rb = targetObject.GetComponent<Rigidbody>();

            // Ensure the target object is initially static
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    public void TriggerFall()
    {
        // Start the coroutine to handle delay and animation
        StartCoroutine(FallWithAnimation());
    }

    private IEnumerator FallWithAnimation()
    {
        // Play push animation
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isPush", true); // Start the push animation
        }

        // Wait for 1 second before applying force
        yield return new WaitForSeconds(0.4f);

        // Apply force to the target object
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(fallDirection.normalized * fallForce, ForceMode.Impulse);
            // Revert player's animation to idle or default
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isPush", false);
            }
        }

        // Stop movement after a delay
        Invoke("StopMovement", 1f); // Stops the object 1 second after it starts moving
    }

    void StopMovement()
    {
        // Stop object's movement and rotation
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // Set back to kinematic to stop
        }

        
    }
}
