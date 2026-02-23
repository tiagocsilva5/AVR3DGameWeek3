using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        Debug.Log(animator);
        
        // Auto-assign camera if not set in Inspector
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        // Start in Idle
        animator.SetFloat("State", 0f);
    }

    void Update()
    {
        // GROUND CHECK
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // INPUT
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // CAMERA RELATIVE MOVEMENT
        Vector3 move = Vector3.zero;

        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            move = forward * z + right * x;
        }
        else
        {
            // Fallback movement if no camera
            move = new Vector3(x, 0f, z);
        }

        move.Normalize(); 

        float moveAmount = new Vector2(x, z).magnitude;

        // ANIMATION
        if (moveAmount > 0.1f)
        {
            animator.SetFloat("State", 1f); // Running
        }
        else
        {
            animator.SetFloat("State", 0f); // Idle
        }

        // MOVE PLAYER
        controller.Move(move * speed * Time.deltaTime);

        // ROTATION
        if (moveAmount > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // JUMP
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // GRAVITY
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
    if (other.CompareTag("Goal"))
    {
        // Optional: stop player movement during transition
        enabled = false;

        GameManager.Instance.PlayerReachedGoal(
            "You have escaped Level 1!\nLoading next level...",
            GameManager.Instance.loadScene
        );
    }
    }
}
