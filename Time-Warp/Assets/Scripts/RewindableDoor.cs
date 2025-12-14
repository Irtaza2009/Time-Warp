using UnityEngine;
using System.Collections.Generic;

public class RewindableDoor : MonoBehaviour
{
    private DoorTrigger doorTrigger;

    private struct DoorState
    {
        public bool isDoorOpen;
        public bool isOpening;
        public Vector3 doorChildPosition;
    }

    private List<DoorState> history = new List<DoorState>();
    private float timer = 0f;
    private float interval = 0.02f;
    private bool wasRewinding = false;

    void Awake()
    {
        doorTrigger = GetComponent<DoorTrigger>();
    }

    void Update()
    {
        // Check if rewinding just stopped
        if (wasRewinding && !RewindManager.IsRewinding)
        {
            doorTrigger.ResumeAfterRewind();
        }
        wasRewinding = RewindManager.IsRewinding;

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

        DoorState s = new DoorState();
        s.isDoorOpen = doorTrigger.IsDoorOpen;
        s.isOpening = doorTrigger.IsOpening;
        s.doorChildPosition = doorTrigger.DoorChildPosition;

        history.Insert(0, s);

        // Keep around ~5 seconds
        if (history.Count > 250)
            history.RemoveAt(history.Count - 1);
    }

    void Rewind()
    {
        if (history.Count == 0) return;

        DoorState s = history[0];
        doorTrigger.RestoreStateFromRewind(s.isDoorOpen, s.isOpening, s.doorChildPosition);

        history.RemoveAt(0);
    }
}
