using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator anim; // Animator reference

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool moving; // Track whether the player is moving

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input (WASD or Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Call animation logic
        Animate();
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.velocity = movement.normalized * moveSpeed;
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
