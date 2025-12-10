using UnityEngine;

public class GhostTrailSpawner : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] float spawnInterval = 0.05f;

    private float timer;

    void Update()
    {
        if (!RewindManager.IsRewinding) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnGhost();
        }
    }

    void SpawnGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);

        // Copy player sprite
        SpriteRenderer original = GetComponent<SpriteRenderer>();
        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();

        ghostSR.sprite = original.sprite;
        ghostSR.flipX = original.flipX;

        // Pass original color to ghost
        GhostFade ghostFade = ghost.GetComponent<GhostFade>();
        if (ghostFade != null)
        {
            ghostFade.SetOriginalColor(original.color);
        }
    }
}
