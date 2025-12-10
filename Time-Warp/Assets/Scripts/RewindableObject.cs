using UnityEngine;
using System.Collections.Generic;

public class RewindableObject : MonoBehaviour
{
    // Store 5 seconds of history at 50 fps so 250 frames
    public float rewindDuration = 5f;
    public bool recordRotation = true;

    private Rigidbody2D rb;
    private bool hasRb;

    private struct Snapshot
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector2 velocity;
    }

    private List<Snapshot> history = new List<Snapshot>();
    private float timeBetweenRecords = 0.02f; // 50 fps
    private float timer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hasRb = rb != null;
    }

    void Update()
    {
        if (RewindManager.IsRewinding)
            Rewind();
        else
            Record();
    }

    // Recording 
    void Record()
    {
        timer += Time.deltaTime;

        // Only record every 0.02 sec for performance
        if (timer < timeBetweenRecords) return;
        timer = 0f;

        if (history.Count > (rewindDuration / timeBetweenRecords))
            history.RemoveAt(history.Count - 1); // Remove oldest

        Snapshot snap = new Snapshot();
        snap.position = transform.position;
        snap.rotation = transform.rotation;
        snap.velocity = hasRb ? rb.linearVelocity : Vector2.zero;

        history.Insert(0, snap);
    }

    // Rewinding
    void Rewind()
    {
        if (history.Count == 0) return;

        // Stop physics while rewinding
        if (hasRb)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector2.zero;
        }

        Snapshot snap = history[0];
        transform.position = snap.position;
        if (recordRotation) transform.rotation = snap.rotation;

        history.RemoveAt(0);
    }

    void LateUpdate()
    {
        if (!RewindManager.IsRewinding && hasRb)
            rb.isKinematic = false;
    }
}
