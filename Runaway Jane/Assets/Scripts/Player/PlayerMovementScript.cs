using Unity.VisualScripting;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed = 5f;               // Normal move speed
    public float sprintSpeedMultiplier = 2f;  // Speed multiplier for sprinting
    public float sneakSpeedMultiplier = 0.5f; // Speed multiplier for sneaking
    private Rigidbody2D rb;
    private Vector2 movement;
    Animator ani;

    private OxySystem oxySystem;              // Reference to the Oxygen System (stamina)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oxySystem = GetComponent<OxySystem>(); // Get OxySystem component
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input (WASD or Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
      
        if (movement.x != 0)
        {
            ani.SetBool("Moving", true);
        }
        else
        {
            ani.SetBool("Moving", false);
        }

        if (movement.y != 0)
        {
            ani.SetBool("Moving", true);
        }
        else
        {
            ani.SetBool("Moving", false);
        }
    }

    void FixedUpdate()
    {
        // Determine current movement speed based on player actions
        float currentMoveSpeed = moveSpeed;

        // Sprinting logic (hold LeftShift)
        if (Input.GetKey(KeyCode.LeftShift) && oxySystem.CanSprint())
        {
            currentMoveSpeed *= sprintSpeedMultiplier; // Increase speed while sprinting
            oxySystem.DrainOxygen(oxySystem.sprintDrainRate * Time.fixedDeltaTime); // Drain oxygen
        }
        // Sneaking logic (hold LeftControl)
        else if (Input.GetKey(KeyCode.LeftControl) && oxySystem.CanSneak())
        {
            currentMoveSpeed *= sneakSpeedMultiplier; // Decrease speed while sneaking
            oxySystem.DrainOxygen(oxySystem.sneakDrainRate * Time.fixedDeltaTime); // Drain oxygen
        }

        // Apply movement to the Rigidbody2D
        rb.velocity = movement.normalized * currentMoveSpeed;
    }
}
