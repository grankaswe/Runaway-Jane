using System;
using System.Collections;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private string healItem = "Bandage"; // Item name in inventory for healing
    [SerializeField] private int healAmount = 20; // Amount healed per bandage
    [SerializeField] private KeyCode healKey = KeyCode.H; // Custom key for healing (default: H)

    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    private Inventory playerInventory;
    private Coroutine bleedCoroutine;

    void Awake()
    {
        currentHealth = maxHealth;
        playerInventory = FindObjectOfType<Inventory>(); // Find the inventory in the scene
    }

    void Update()
    {
        if (Input.GetKeyDown(healKey)) // Press assigned key to heal
        {
            UseBandage();
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} took {amount} damage! Health: {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} healed {amount} HP. Health: {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject); // Destroy the player
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    public void UseBandage()
    {
        if (currentHealth < maxHealth) // Only heal if health is less than max
        {
            if (playerInventory.HasItem(healItem)) // Check if the player has a bandage
            {
                playerInventory.RemoveItem(healItem); // Remove one bandage from inventory

                // Heal the player by restoring 75% of max health
                int healAmount = Mathf.CeilToInt(maxHealth * 0.75f);
                Heal(healAmount); // Heal the player
                Debug.Log("Used a Bandage! Current Health: " + currentHealth);

                Debug.Log("Used a Bandage! Restored " + healAmount + " HP. Current Health: " + currentHealth);
            }
            else
            {
                Debug.Log("No bandages left!");
            }
        }
        else
        {
            Debug.Log("Health is already full!");
        }
    }

    // Add the StartBleeding method here to handle the bleeding effect
    public void StartBleeding(int bleedDamage, float bleedDuration, float bleedInterval)
    {
        if (bleedCoroutine != null)
        {
            StopCoroutine(bleedCoroutine); // Stop any ongoing bleeding effect
        }

        bleedCoroutine = StartCoroutine(BleedingEffect(bleedDamage, bleedDuration, bleedInterval));
    }

    private IEnumerator BleedingEffect(int bleedDamage, float bleedDuration, float bleedInterval)
    {
        float elapsedTime = 0f;
        while (elapsedTime < bleedDuration)
        {
            TakeDamage(bleedDamage); // Apply bleeding damage to the player
            elapsedTime += bleedInterval;
            yield return new WaitForSeconds(bleedInterval); // Wait for the next damage tick
        }
    }
}
