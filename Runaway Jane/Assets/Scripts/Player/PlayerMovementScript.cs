using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed = 5f;               // Normal move speed
    public float sprintSpeedMultiplier = 2f;  // Speed multiplier for sprinting
    public float sneakSpeedMultiplier = 0.5f; // Speed multiplier for sneaking
    public Animator anim;                     // Animator reference

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool moving;                      // Track whether the player is moving
    private OxygenSystem oxygenSystem;        // Reference to the Oxygen System (stamina)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oxygenSystem = GetComponent<OxygenSystem>(); // Get OxygenSystem component
    }

    void Update()
    {
        // Get input (WASD or Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Handle animation updates
        Animate();
    }

    void FixedUpdate()
    {
        // Get current movement speed based on sprinting or sneaking
        float currentMoveSpeed = moveSpeed;

        // Check if player is sprinting (hold LeftShift)
        if (Input.GetKey(KeyCode.LeftShift) && oxygenSystem.CanSprint())
        {
            currentMoveSpeed *= sprintSpeedMultiplier; // Increase speed while sprinting
            oxygenSystem.DrainOxygen(oxygenSystem.sprintDrainRate * Time.deltaTime); // Drain oxygen while sprinting
        }
        // Check if player is sneaking (hold LeftControl)
        else if (Input.GetKey(KeyCode.LeftControl) && oxygenSystem.CanSneak())
        {
            currentMoveSpeed *= sneakSpeedMultiplier; // Decrease speed while sneaking
            oxygenSystem.DrainOxygen(oxygenSystem.sneakDrainRate * Time.deltaTime); // Drain oxygen while sneaking
        }

        // Apply movement
        rb.velocity = movement.normalized * currentMoveSpeed;
    }

    void Animate()
    {
        // Check if the player is moving
        if (movement.magnitude > 0.1f || movement.magnitude < -0.1f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        // Update animation parameters
        if (moving)
        {
            anim.SetFloat("X", movement.x);
            anim.SetFloat("Y", movement.y);
        }

        anim.SetBool("Moving", moving);
    }
}
