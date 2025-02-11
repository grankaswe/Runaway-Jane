using System.Collections;
using UnityEngine;

public class Enemyatk : MonoBehaviour
{
    [SerializeField] private int initialDamage = 10;   // Damage on first hit
    [SerializeField] private int bleedDamage = 5;      // Damage per tick
    [SerializeField] private float bleedDuration = 5f; // Total bleed duration
    [SerializeField] private float bleedInterval = 1f; // Time between ticks

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            HP playerHealth = collision.collider.GetComponent<HP>();
            if (playerHealth != null)
            {
                // Apply initial damage
                playerHealth.TakeDamage(initialDamage);
                Debug.Log($"Enemy hit player! Initial damage: {initialDamage}. Player Health: {playerHealth.GetCurrentHealth()}");

                // Start bleeding effect
                if (playerHealth.GetCurrentHealth() > 0)
                {
                    playerHealth.StartBleeding(bleedDamage, bleedDuration, bleedInterval);
                }
            }
        }
    }
}
