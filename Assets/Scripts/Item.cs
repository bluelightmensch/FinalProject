using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Make sure the collider is the player
        if (other.CompareTag("Player"))
        {
            // Set the flag on the GameManager or global static class
            GameManager.instance.itemDestroyed = true;
            // or GlobalFlags.ItemDestroyed = true;

            // Destroy this Trigger object
            Destroy(gameObject);
        }
    }
}