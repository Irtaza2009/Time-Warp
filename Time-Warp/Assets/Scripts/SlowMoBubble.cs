using UnityEngine;
using System.Collections.Generic;

public class SlowMoBubble : MonoBehaviour
{
    public static SlowMoBubble Instance;

    [Header("Bubble Settings")]
    public float radius = 4f;
    public float slowFactor = 0.2f; // 20% speed
    public bool isActive = false;
    [SerializeField] float slowmoDuration = 5f;

    private Transform player;
    private List<Rigidbody2D> affectedBodies = new List<Rigidbody2D>();
    public AbilityUI slowmoUI;
    private float slowmoTimer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && slowmoUI.IsReady)
        {
            Activate();
        }

        if (isActive && Input.GetKey(KeyCode.E))
        {
            slowmoTimer -= Time.deltaTime;
            slowmoUI.DecreaseTimer(Time.deltaTime);
            if (slowmoTimer <= 0)
            {
                Deactivate();
                slowmoUI.TriggerCooldown();
            }
            else
            {
                ApplyBubbleSlowmo();
            }
        }
        else if (isActive)
        {
            Deactivate();
            slowmoUI.TriggerCooldown();
        }
    }

    void Activate()
    {
        isActive = true;
        slowmoTimer = slowmoDuration;
    }

    void Deactivate()
    {
        isActive = false;

        // restore velocities
        foreach (var rb in affectedBodies)
        {
            if (rb != null)
                rb.linearDamping = 0f;
        }
        affectedBodies.Clear();
    }

    void ApplyBubbleSlowmo()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, radius);

        affectedBodies.Clear();

        foreach (Collider2D hit in hits)
        {
            Rigidbody2D rb = hit.attachedRigidbody;
            if (rb == null) continue;

            // Ignore player physic
            if (hit.CompareTag("Player")) continue;

            affectedBodies.Add(rb);

            // Apply drag-based slow motion
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, 25f * (1f - slowFactor), Time.deltaTime * 15f);
        }
    }

    void OnDrawGizmos()
    {
        // Visualize bubble
        Gizmos.color = new Color(0, 0.6f, 1f, 0.2f);
        if (player != null)
            Gizmos.DrawSphere(player.position, radius);
    }
}
