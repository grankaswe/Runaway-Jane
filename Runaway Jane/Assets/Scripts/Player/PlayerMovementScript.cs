using Unity.VisualScripting;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed = 5f;               // Normal move speed
    public float sprintSpeedMultiplier = 2f;  // Speed multiplier for sprinting
    public float sneakSpeedMultiplier = 0.5f; // Speed multiplier for sneaking
    public float hurtSpeedMultiplier = 0.5f;  // Speed multiplier when the player is hurt
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private OxySystem oxySystem;              // Reference to the Oxygen System (stamina)
    private HP playerHealth;                  // Reference to the player's HP script

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oxySystem = GetComponent<OxySystem>(); // Get OxySystem component
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<HP>();     // Get the player's health component
    }

    void Update()
    {
        // Get input (WASD or Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
        if (movement.magnitude > 1)
            movement.Normalize();

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("IsMoving", movement.magnitude > 0);
    }

    void FixedUpdate()
    {
        // Determine current movement speed based on player actions
        float currentMoveSpeed = moveSpeed;
        bool isSprinting = false;
        bool isHurt = playerHealth.GetCurrentHealth() < playerHealth.GetMaxHealth() * 0.5f;
        bool isLimping = isHurt && movement.magnitude > 0;

        // Sprinting logic (hold LeftShift)
        if (Input.GetKey(KeyCode.LeftShift) && oxySystem.CanSprint() && !isHurt)
        {
            currentMoveSpeed *= sprintSpeedMultiplier; // Increase speed while sprinting
            oxySystem.DrainOxygen(oxySystem.sprintDrainRate * Time.fixedDeltaTime); // Drain oxygen
            isSprinting = true;
        }
        // Sneaking logic (hold LeftControl)
        else if (Input.GetKey(KeyCode.LeftControl) && oxySystem.CanSneak())
        {
            currentMoveSpeed *= sneakSpeedMultiplier; // Decrease speed while sneaking
            oxySystem.DrainOxygen(oxySystem.sneakDrainRate * Time.fixedDeltaTime); // Drain oxygen
        }
        // If hurt, apply movement penalty
        if (isHurt)
        {
            currentMoveSpeed *= hurtSpeedMultiplier; // Reduce speed if health is below 50%
        }

        // Apply movement to the Rigidbody2D
        rb.velocity = movement.normalized * currentMoveSpeed;

        // Update animator parameters
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("IsHurt", isHurt);
        animator.SetBool("IsLimping", isLimping);
    }
}
