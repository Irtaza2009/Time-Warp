using UnityEngine;

public class SpikeSpawnerTrigger : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] GameObject spikePrefab;
    [SerializeField] int spikeCount = 6;
    [SerializeField] float horizontalSpacing = 1f;
    [SerializeField] float spawnHeightOffset = 0.5f;
    [SerializeField] float cooldown = 2f;

    float lastSpawnTime = -999f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time < lastSpawnTime + cooldown) return;
        if (!other.CompareTag("Player")) return;

        lastSpawnTime = Time.time;
        Debug.Log("Spike Spawner Triggered");
        SpawnSpikes();
    }

    void SpawnSpikes()
    {
        // Start from the left edge of the trigger object
        Collider2D col = GetComponent<Collider2D>();
        Vector3 startPos = transform.position;
        
        if (col != null)
        {
            startPos.x = col.bounds.min.x;
        }

        for (int i = 0; i < spikeCount; i++)
        {
            Vector3 spawnPos = startPos;
            spawnPos.x += i * horizontalSpacing;
            spawnPos.y += spawnHeightOffset;

            Instantiate(spikePrefab, spawnPos, Quaternion.Euler(0, 0, 180));
        }
    }
}
