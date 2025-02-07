using UnityEngine;

public class OxygenSystem : MonoBehaviour
{
    public float maxOxygen = 100f;               // Maximum oxygen (stamina)
    public float currentOxygen;                  // Current oxygen level
    public float sprintDrainRate = 10f;          // Oxygen drain per second while sprinting
    public float sneakDrainRate = 5f;            // Oxygen drain per second while sneaking
    public float oxygenRegenRate = 5f;           // Oxygen regeneration rate per second when idle

    void Start()
    {
        currentOxygen = maxOxygen; // Initialize oxygen
    }

    void Update()
    {
        // Regenerate oxygen if not sprinting or sneaking
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && currentOxygen < maxOxygen)
        {
            RegenerateOxygen();
        }
    }

    // Check if player can sprint (only if oxygen is available)
    public bool CanSprint()
    {
        return currentOxygen > 0;
    }

    // Check if player can sneak (only if oxygen is available)
    public bool CanSneak()
    {
        return currentOxygen > 0;
    }

    // Drain oxygen when sprinting or sneaking
    public void DrainOxygen(float amount)
    {
        currentOxygen -= amount;

        if (currentOxygen < 0)
        {
            currentOxygen = 0; // Prevent oxygen from going negative
        }
    }

    // Regenerate oxygen when idle
    void RegenerateOxygen()
    {
        currentOxygen += oxygenRegenRate * Time.deltaTime;

        if (currentOxygen > maxOxygen)
        {
            currentOxygen = maxOxygen; // Prevent oxygen from exceeding maximum
        }
    }

    // Optional: Display current oxygen for debugging
    void OnGUI()
    {
        GUILayout.Label("Current Oxygen: " + currentOxygen.ToString("F2"));
    }
}
