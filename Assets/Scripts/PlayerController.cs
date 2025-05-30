using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private InputAction _moveIA;
    [SerializeField] private InputAction _jumpIA;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _sensitivity;
    private Vector3 _moveInput;
    private Vector2 _cameraInput;
    private float _xRotation;
    private float _yRotation;

    private void OnEnable()
    {
        _moveIA.Enable();
        _jumpIA.Enable();
    }

    private void OnDisable()
    {
        _moveIA.Disable();
        _jumpIA.Disable();
    }

    void Update()
    {
        _moveInput = new Vector3(_moveIA.ReadValue<Vector2>().x, 0f, _moveIA.ReadValue<Vector2>().y);
        _cameraInput = Mouse.current.delta.ReadValue();

        if (_jumpIA.triggered)
        {
            Jump();
        }

        MovePlayer();
        MoveCamera();
    }

    private void MovePlayer()
    {
        Vector3 _moveVector = transform.TransformDirection(_moveInput) * _moveSpeed;
        _rb.linearVelocity = new Vector3(_moveVector.x, _rb.linearVelocity.y, _moveVector.z);
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
    }

    private void MoveCamera()
    {
        _xRotation -= _cameraInput.y * _sensitivity;
        //limit camera from rotating upside down
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _yRotation -= _cameraInput.x * _sensitivity * -1;

        transform.Rotate(0f, _cameraInput.x * _sensitivity, 0f);
        _camera.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }
}
