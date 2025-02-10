using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private string keyName = "Key"; // Set key name in the Inspector
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Interaction key
    private Inventory playerInventory;
    private bool playerNearby = false;
    private GameObject player;

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>(); // Find inventory in the scene
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            if (playerInventory != null && playerInventory.AddItem(keyName))
            {
                Destroy(gameObject); // Remove key from scene
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
            player = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
