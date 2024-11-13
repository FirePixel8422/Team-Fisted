using UnityEngine;


public class Movement : MonoBehaviour
{
    public static Movement Instance;



    public float walkSpeed;
    [HideInInspector]
    public float moveSpeed;
    public float sprintSpeed;
    public float accelSpeed;

    public bool sprinting;

    private float maxStamina;
    public float stamina;

    public float staminaTime;
    public float staminaRegenSpeed;


    public float rotationSpeed;


    private Vector2 mouse;

    public Camera camera;
    public Rigidbody rb;


    private void Awake()
    {
        Instance = this;

        maxStamina = stamina;

        moveSpeed = walkSpeed;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Vector2 moveDir = new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"));
        Vector2 mousemovement = new Vector2(UnityEngine.Input.GetAxisRaw("Mouse X"), UnityEngine.Input.GetAxisRaw("Mouse Y"));


        Move(moveDir);

        if (mousemovement != Vector2.zero)
        {
            Rotate(mousemovement);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeSprintState(true);
        }
        else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift))
        {
            ChangeSprintState(false);
        }
    }

    public void Move(Vector2 moveDir)
    {
        Vector3 moveDirection = transform.forward * moveDir.y + transform.right * moveDir.x;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);

        rb.velocity = moveDirection * moveSpeed;

        if(moveDir != Vector2.zero && sprinting)
        {
            if (stamina <= 0)
            {
                ChangeSprintState(false);
            }

            else
            {
                stamina -= Time.deltaTime * (maxStamina / staminaTime);
            }
        }
        else if (stamina < maxStamina)
        {
            stamina += Time.deltaTime * staminaRegenSpeed;
        }
    }

    public void SpeedControl()
    {
        Vector3 speed = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (speed.magnitude > moveSpeed)
        {
            Vector3 speedLimited = speed.normalized * moveSpeed;
            rb.velocity = new Vector3(speedLimited.x, rb.velocity.y, speedLimited.z);
        }
    }

    public void Rotate(Vector2 mouseMovement)
    {
        float xB = mouseMovement.x * rotationSpeed;
        float yB = mouseMovement.y * rotationSpeed;

        float x = xB + mouse.x;
        float y = -yB + mouse.y;

        y = Mathf.Clamp(y, -85, 85);

        mouse.x = x;
        mouse.y = y;

        transform.localRotation = Quaternion.Euler(0, x, 0);
        camera.transform.localRotation = Quaternion.Euler(y, 0, 0);
    }



    public void ChangeSprintState(bool state)
    {
        if (state == sprinting)
        {
            return;
        }

        sprinting = state;


        if (sprinting)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }
}
