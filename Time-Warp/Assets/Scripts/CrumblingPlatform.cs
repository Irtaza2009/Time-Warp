using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] float crumbleDelay = 3f;
    [SerializeField] string playerTag = "Player";

    Collider2D platformCollider;
    SpriteRenderer platformRenderer;
    bool crumbleStarted;

    void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (crumbleStarted) return;
        if (!collision.collider.CompareTag(playerTag)) return;

        crumbleStarted = true;
        StartCoroutine(CrumbleAfterDelay());
    }

    IEnumerator CrumbleAfterDelay()
    {
        yield return new WaitForSeconds(crumbleDelay);

        if (platformCollider != null) platformCollider.enabled = false;
        if (platformRenderer != null) platformRenderer.enabled = false;
    }

    void ResetPlatform()
    {
        if (platformCollider != null) platformCollider.enabled = true;
        if (platformRenderer != null) platformRenderer.enabled = true;
        crumbleStarted = false;
    }
}
