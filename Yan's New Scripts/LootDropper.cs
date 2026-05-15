using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [Header("Guaranteed Drop (100%)")]
    [SerializeField] private GameObject guaranteedLoot; // Put the Coin prefab here

    [System.Serializable]
    public class BonusLoot
    {
        public GameObject itemPrefab; // Health potion or buff prefab
        [Range(0f, 100f)]
        public float dropChance;      // Chance from 0 to 100
    }

    [Header("Bonus Drops (Small Chance)")]
    [SerializeField] private BonusLoot[] bonusLootTable;

    // Unity automatically calls this function when the object is destroyed (dies)
    private void OnDestroy()
    {
        // Protection against spawning loot when just changing levels or closing the game
        if (!gameObject.scene.isLoaded) return;

        DropItems();
    }

    private void DropItems()
    {
        // 1. Spawn the guaranteed coin (if a prefab is assigned)
        if (guaranteedLoot != null)
        {
            // Add a slight coordinate offset so items don't drop in the exact same spot
            Vector3 dropPosition = transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
            Instantiate(guaranteedLoot, dropPosition, Quaternion.identity);
        }

        // 2. Check if any bonus loot should drop
        foreach (BonusLoot bonus in bonusLootTable)
        {
            float randomValue = Random.Range(0f, 100f);

            if (randomValue <= bonus.dropChance)
            {
                Vector3 bonusPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                Instantiate(bonus.itemPrefab, bonusPosition, Quaternion.identity);

                // IMPORTANT: If you want ONLY ONE bonus to drop (e.g., health OR buff, but not both at once),
                // uncomment the 'return' statement below:
                return;
            }
        }
    }
}