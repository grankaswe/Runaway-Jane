using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyatk : MonoBehaviour
{
    [SerializeField] private int initialDamage = 10;   // Damage on first hit
    [SerializeField] private int bleedDamage = 5;      // Damage per tick
    [SerializeField] private float bleedDuration = 5f; // Total bleed duration
    [SerializeField] private float bleedInterval = 1f; // Time between ticks
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HP playerHealth = other.GetComponent<HP>();
            if (playerHealth != null)
            {
                // Apply initial damage
                playerHealth.TakeDamage(initialDamage);
                Debug.Log($"Enemy hit player! Initial damage: {initialDamage}. Player Health: {playerHealth.GetCurrentHealth()}");

                // Start bleeding effect
                if (playerHealth.GetCurrentHealth() > 0)
                {
                    StartCoroutine(ApplyBleed(playerHealth));
                }
            }
        }
    }

    private IEnumerator ApplyBleed(HP playerHealth)
    {
        float elapsedTime = 0f;
        while (elapsedTime < bleedDuration)
        {
            yield return new WaitForSeconds(bleedInterval); // Wait before applying bleed

            if (playerHealth == null) yield break; // Stop if player is destroyed

            playerHealth.TakeDamage(bleedDamage);
            Debug.Log($"Player took {bleedDamage} bleed damage. Player Health: {playerHealth.GetCurrentHealth()}");

            if (playerHealth.GetCurrentHealth() <= 0)
            {
                Debug.Log("Player has died!");
                Destroy(playerHealth.gameObject); // Destroy player
                yield break; // Stop coroutine
            }

            elapsedTime += bleedInterval;
        }
    }
}
