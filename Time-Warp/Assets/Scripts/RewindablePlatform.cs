using UnityEngine;
using System.Collections.Generic;

public class RewindablePlatform : MonoBehaviour
{
    private CrumblingPlatform cp;

    private struct PlatformState
    {
        public bool colliderEnabled;
        public bool rendererEnabled;
        public bool crumbleStarted;
    }

    List<PlatformState> history = new List<PlatformState>();
    float timer = 0f;
    float interval = 0.02f;

    void Awake()
    {
        cp = GetComponent<CrumblingPlatform>();
    }

    void Update()
    {
        if (RewindManager.IsRewinding)
            Rewind();
        else
            Record();
    }

    void Record()
    {
        timer += Time.deltaTime;
        if (timer < interval) return;
        timer = 0f;

        PlatformState s = new PlatformState();
        s.colliderEnabled = cp.PlatformColliderEnabled;
        s.rendererEnabled = cp.PlatformVisible;
        s.crumbleStarted = cp.HasCrumbleStarted;

        history.Insert(0, s);

        // Keep around ~5 seconds
        if (history.Count > 250)
            history.RemoveAt(history.Count - 1);
    }

    void Rewind()
    {
        if (history.Count == 0) return;

        PlatformState s = history[0];
        cp.RestoreStateFromRewind(s.colliderEnabled, s.rendererEnabled, s.crumbleStarted);

        history.RemoveAt(0);
    }
}
