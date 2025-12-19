using UnityEngine;
using System.Collections;

public class SpikeSpawnerTrigger : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] GameObject spikePrefab;
    [SerializeField] int spikeCount = 6;
    [SerializeField] float horizontalSpacing = 1f;
    [SerializeField] float spawnHeightOffset = 0.5f;
    [SerializeField] float cooldown = 2f;
    [SerializeField] float rotationAngle = 180f;
    [SerializeField] bool spawnContinuously = false;

    float lastSpawnTime = -999f;

    void Update()
    {
        if (!spawnContinuously) return;

        if (Time.time >= lastSpawnTime + cooldown)
        {
            lastSpawnTime = Time.time;
            SpawnSpikes();
            StartCoroutine(ScreenShake(0.3f, 0.2f));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time < lastSpawnTime + cooldown) return;
        if (!other.CompareTag("Player")) return;

        lastSpawnTime = Time.time;
        Debug.Log("Spike Spawner Triggered");
        SpawnSpikes();
        StartCoroutine(ScreenShake(0.3f, 0.2f));
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

            Instantiate(spikePrefab, spawnPos, Quaternion.Euler(0, 0, rotationAngle));
        }
    }

    IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            Camera.main.transform.position = originalPos + new Vector3(x, y, 0);
            yield return null;
        }

        Camera.main.transform.position = originalPos;
    } 
}
