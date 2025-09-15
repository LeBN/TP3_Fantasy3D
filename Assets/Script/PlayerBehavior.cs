using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] InputSystem_Actions inputs;
    [SerializeField] Transform _cameraRig;
    [SerializeField] Transform _cameraRig2;
    [SerializeField] Animator animator;
    [SerializeField] float timeBeforeJump = 0.5f;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] bool isGrounded = false;
    [SerializeField] bool isJumping = false;
    private Vector2 moves = new Vector2();
    private Vector2 look = new Vector2();
    private Vector2 lookInput;
    private float xRot = 0;
    private float yRot = 0;
    private uint doPlayerRotate = 0;

    void Start()
    {
        rb.useGravity = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        if (Cursor.lockState == CursorLockMode.Locked)
            look = context.ReadValue<Vector2>();
        else
            look = Vector2.zero;
    }

    private void Awake()
    {
        inputs = new InputSystem_Actions();

        inputs.Player.Jump.performed += ctx => Jump();

        inputs.Player.Move.performed += ctx => moves = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moves = Vector2.zero;

        inputs.Player.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        inputs.Player.Look.canceled += ctx => look = Vector2.zero;

        inputs.Player.PlayerRotation.performed += ctx => doPlayerRotate = 1;
        inputs.Player.PlayerRotation.canceled += ctx => doPlayerRotate = 0;
        inputs.Player.CameraRotation.performed += ctx => doPlayerRotate = 2;
        inputs.Player.CameraRotation.canceled += ctx => doPlayerRotate = 0;
    }

    void Update()
    {
        if (moves != Vector2.zero)
        {
            transform.Translate(new Vector3(moves.x, 0f, moves.y) * Time.deltaTime * speed);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        // Mouse movements
        if (doPlayerRotate == 1)
        {
            float yaw = look.x * mouseSensitivity * Time.deltaTime;
            float pitch = look.y * mouseSensitivity * Time.deltaTime;

            xRot = Mathf.Clamp(xRot - pitch, -29f, 65f);
            yRot = yRot - yaw;

            transform.Rotate(Vector3.up * yaw);
            _cameraRig.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }
        else if (doPlayerRotate == 2)
        {
            float yaw = look.x * mouseSensitivity * Time.deltaTime;
            float pitch = look.y * mouseSensitivity * Time.deltaTime;

            xRot = Mathf.Clamp(xRot - pitch, -29f, 65f);
            yRot = yRot - yaw;
            _cameraRig.localRotation = Quaternion.Euler(xRot, -yRot, 0f);
        }
    }

    void OnEnable()
    {
        inputs.Player.Enable();
    }

    void OnDisable()
    {
        inputs.Player.Disable();
    }

    void Jump()
    {
        if (isGrounded && !isJumping) {
            isJumping = true;
            animator.SetBool("Walk", false);
            animator.SetTrigger("Jump");
            StartCoroutine(JumpAfter(timeBeforeJump));
        }
    }

    private System.Collections.IEnumerator JumpAfter(float time)
    {
        yield return new WaitForSeconds(time);
        yield return new WaitForFixedUpdate();
        rb.linearVelocity += Vector3.up * jumpForce;
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    isGrounded = true;
                }
            }
        }
    }
}
