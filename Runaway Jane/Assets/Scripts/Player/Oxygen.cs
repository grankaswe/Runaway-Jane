using UnityEngine;

public class OxySystem : MonoBehaviour
{
    public float maxOxygen = 100f;               // Maximum oxygen (stamina)
    public float currentOxygen;                 // Current oxygen level
    public float sprintDrainRate = 10f;         // Oxygen drain per second while sprinting
    public float sneakDrainRate = 5f;           // Oxygen drain per second while sneaking
    public float oxygenRegenRate = 5f;          // Oxygen regeneration rate per second when idle

    void Start()
    {
        currentOxygen = maxOxygen; // Initialize oxygen level
    }

    void Update()
    {
        // Regenerate oxygen when neither sprinting nor sneaking
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && currentOxygen < maxOxygen)
        {
            RegenerateOxygen();
        }
    }

    // Check if the player can sprint (oxygen > 0)
    public bool CanSprint()
    {
        return currentOxygen > 0;
    }

    // Check if the player can sneak (oxygen > 0)
    public bool CanSneak()
    {
        return currentOxygen > 0;
    }

    // Drain oxygen (used by TopDownMovement script)
    public void DrainOxygen(float amount)
    {
        currentOxygen -= amount;
        if (currentOxygen < 0)
        {
            currentOxygen = 0; // Prevent oxygen from going below zero
        }
    }

    // Regenerate oxygen when idle
    private void RegenerateOxygen()
    {
        currentOxygen += oxygenRegenRate * Time.deltaTime;
        if (currentOxygen > maxOxygen)
        {
            currentOxygen = maxOxygen; // Cap oxygen at maximum
        }
    }

    // Optional: Display current oxygen level for debugging
    void OnGUI()
    {
        GUILayout.Label("Current Oxygen: " + currentOxygen.ToString("F2"));
    }
}
