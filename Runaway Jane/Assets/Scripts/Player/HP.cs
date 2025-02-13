using System;
using System.Collections;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private string healItem = "Bandage"; // Item name in inventory for healing
    [SerializeField] private KeyCode healKey = KeyCode.H; // Custom key for healing (default: H)

    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    private Inventory playerInventory;

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

    public void StartBleeding(int damagePerTick, float duration, float interval)
    {
        StartCoroutine(BleedCoroutine(damagePerTick, duration, interval));
    }

    private IEnumerator BleedCoroutine(int damagePerTick, float duration, float interval)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            TakeDamage(damagePerTick);
            yield return new WaitForSeconds(interval);
            elapsedTime += interval;
        }
    }
}
