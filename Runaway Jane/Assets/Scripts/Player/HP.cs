using System;
using System.Collections;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private string healItem = "Bandage"; // Item name in inventory for healing
    [SerializeField] private int healAmount = 20; // Amount healed per bandage
    [SerializeField] private KeyCode healKey = KeyCode.H; // Key for healing

    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    private Inventory playerInventory;
    private bool isBleeding = false;
    private Coroutine bleedCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        playerInventory = FindObjectOfType<Inventory>(); // Find inventory in scene
    }

    void Update()
    {
        if (Input.GetKeyDown(healKey)) // Press key to heal
        {
            UseBandage();
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

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
        if (amount <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} healed {amount} HP. Health: {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // Destroy the player
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    public void UseBandage()
    {
        if (playerInventory == null)
        {
            Debug.LogError("Inventory not found!");
            return;
        }

        if (currentHealth >= maxHealth && !isBleeding)
        {
            Debug.Log("Health is already full and you're not bleeding!");
            return;
        }

        if (playerInventory.HasItem(healItem))
        {
            playerInventory.RemoveItem(healItem); // Remove bandage from inventory
            Heal(healAmount); // Heal the player

            if (isBleeding)
            {
                StopBleeding();
            }

            Debug.Log($"Used a {healItem}! Current Health: {currentHealth}");
        }
        else
        {
            Debug.Log($"No {healItem}s left!");
        }
    }

    public void StartBleeding(float damagePerTick, float duration, float tickInterval)
    {
        if (isBleeding) return; // Prevent multiple bleeding instances

        isBleeding = true;
        bleedCoroutine = StartCoroutine(BleedingEffect(damagePerTick, duration, tickInterval));
        Debug.Log("Player started bleeding!");
    }

    private IEnumerator BleedingEffect(float damagePerTick, float duration, float tickInterval)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(tickInterval);

            if (!isBleeding) yield break; // Stop if bleeding was canceled

            TakeDamage((int)damagePerTick);
            Debug.Log($"Bleeding! Took {damagePerTick} damage.");

            if (currentHealth <= 0) yield break; // Stop bleeding if dead

            elapsedTime += tickInterval;
        }

        isBleeding = false;
        Debug.Log("Bleeding stopped naturally.");
    }

    public void StopBleeding()
    {
        if (isBleeding)
        {
            isBleeding = false;
            if (bleedCoroutine != null)
            {
                StopCoroutine(bleedCoroutine);
                bleedCoroutine = null;
            }
            Debug.Log("Bleeding stopped using bandage!");
        }
    }
}
