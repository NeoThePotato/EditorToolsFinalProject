using UnityEngine;
using UnityEngine.InputSystem;

public class MyCharacterController : MonoBehaviour
{
    public Camera camera;

    public float moveSpeed = 5f;
    public bool canJump = true;
    public float jumpHeight = 2f;
    public bool customSensitivity = true;
    public float sensitivity = 0.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private bool isGrounded;

    private Vector2 cameraInput;
    private float xRotation;
    private float yRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (!customSensitivity)
        {
            sensitivity = 0.5f;
        }
    }

    void Update()
    {
        //movement controls
        MovePlayer();

        //camera controls
        cameraInput = Mouse.current.delta.ReadValue();
        MoveCamera();
    }

    private void MovePlayer()
    {
        isGrounded = controller.isGrounded;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (canJump && Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void MoveCamera()
    {
        xRotation -= cameraInput.y * sensitivity;
        //limit camera from rotating upside down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation -= cameraInput.x * sensitivity * -1;

        transform.Rotate(0f, cameraInput.x * sensitivity, 0f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}