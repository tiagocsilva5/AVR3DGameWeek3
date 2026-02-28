using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
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

    [Header("Goal Settings")]
    [SerializeField] private string nextScene = "Scene_2";
    [SerializeField] private string goalMessage = "You have escaped Level 1!\nCongrats!";

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        // Auto-assign camera if not set
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        // Start idle animation
        if (animator != null)
            animator.SetFloat("State", 0f);
    }

    void Update()
    {
        if (!enabled) return; // Stop updates if movement disabled

        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        // Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Camera-relative movement
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
            move = new Vector3(x, 0f, z);
        }

        move.Normalize();
        float moveAmount = new Vector2(x, z).magnitude;

        // Animation
        if (animator != null)
            animator.SetFloat("State", moveAmount > 0.1f ? 1f : 0f);

        // Move player
        controller.Move(move * speed * Time.deltaTime);

        // Rotation
        if (moveAmount > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Goal 1: normal level completion
        if (other.CompareTag("Goal"))
        {
            enabled = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadSceneWithMessage(nextScene, goalMessage);
            }
            else
            {
                Debug.LogError("GameManager.Instance is null!");
            }
        }

        // Goal2: escaped prison message, stay in current scene
        if (other.CompareTag("Goal2"))
        {
            enabled = false;

            if (GameManager.Instance != null)
            {
                string currentScene = SceneManager.GetActiveScene().name;
                GameManager.Instance.LoadSceneWithMessage(currentScene, "You escaped the prison!\nWell done!");
            }
            else
            {
                Debug.LogError("GameManager.Instance is null!");
            }
        }
    }
}