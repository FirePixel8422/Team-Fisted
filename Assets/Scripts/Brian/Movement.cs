using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Input _input;

    InputAction _move;
    InputAction _rotate;

    public float _moveSpeed;
    //public float _sprintSpeed;

    public float _rotationSpeed;

    public Components _components = new Components();

    [Serializable]
    public class Components
    {
        public Camera _camera;
        public Rigidbody _rb;
    }

    float _x, _y;

    private void Awake()
    {
        _input = new Input();
    }

    private void OnEnable()
    {
        _input.Enable();

        _move = _input.Movement.Move;
        _rotate = _input.Movement.Rotate;
    }

    private void OnDisable()
    {
        _input.Disable();

        _move = null;
        _rotate = null;
    }

    private void Update()
    {
        Move(_move.ReadValue<Vector2>());
        Rotate(_rotate.ReadValue<Vector2>());
    }

    public void Move(Vector2 context)
    {
        Vector3 _moveDirection = (transform.forward * context.y + transform.right * context.x) * Time.deltaTime;
        _moveDirection = new Vector3(_moveDirection.x, 0, _moveDirection.z);

        _components._rb.AddForce(_moveDirection * 3000);

        SpeedControle();
    }

    public void SpeedControle()
    {
        Vector3 _speed = new Vector3(_components._rb.velocity.x, 0, _components._rb.velocity.z);

        if (_speed.magnitude > _moveSpeed)
        {
            Vector3 _speedLimited = _speed.normalized * _moveSpeed;
            _components._rb.velocity = new Vector3(_speedLimited.x, _components._rb.velocity.y, _speedLimited.z);
        }
    }

    public void Rotate(Vector2 context)
    {
        float _xB = context.x * _rotationSpeed;
        float _yB = context.y * _rotationSpeed;

        _x += _xB;
        _y -= _yB;

        _y = Math.Clamp(_y, -85, 85);

        transform.localRotation = Quaternion.Euler(0, _x, 0);
        _components._camera.transform.localRotation = Quaternion.Euler(_y, 0, 0);
    }
}
