using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] float crumbleDelay = 3f;
    [SerializeField] string playerTag = "Player";

    public Collider2D platformCollider { get; private set; }
    public SpriteRenderer platformRenderer { get; private set; }
    public bool crumbleStarted { get; private set; }

    public bool PlatformColliderEnabled => platformCollider.enabled;
    public bool PlatformVisible => platformRenderer.enabled;
    public bool HasCrumbleStarted => crumbleStarted;

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
        platformCollider.enabled = false;
        platformRenderer.enabled = false;
    }

    public void RestoreStateFromRewind(bool colliderState, bool rendererState, bool startedState)
    {
        platformCollider.enabled = colliderState;
        platformRenderer.enabled = rendererState;
        crumbleStarted = startedState;
    }
}
