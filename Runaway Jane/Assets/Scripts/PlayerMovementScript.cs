using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed = 5f;            // Normal move speed
    public float sprintSpeedMultiplier = 2f; // Speed multiplier for sprinting
    public float sneakSpeedMultiplier = 0.5f; // Speed multiplier for sneaking
    private Rigidbody2D rb;
    private Vector2 movement;

    private OxygenSystem oxygenSystem;      // Reference to the Oxygen System (stamina)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oxygenSystem = GetComponent<OxygenSystem>(); // Get OxygenSystem component
    }

    void Update()
    {
        // Get input (WASD or Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
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

        // Apply movement with velocity
        rb.velocity = movement.normalized * currentMoveSpeed;
    }
}
