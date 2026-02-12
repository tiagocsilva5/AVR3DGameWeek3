using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public Rigidbody rb;
    public Transform cameraTransform;

    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // --- Camera-relative movement ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        moveDirection = forward * vertical + right * horizontal;

        rb.AddForce(moveDirection * speed, ForceMode.Force);

        // --- Jump ---
        if(Input.GetButton("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Prevent double jump
        }
    }

    // --- Ground Check ---
    void OnCollisionStay(Collision collision)
    {
        // Consider player grounded if touching floor
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}