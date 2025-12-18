using UnityEngine;
using System.Collections.Generic;

public class RewindManager : MonoBehaviour
{
    public static bool IsRewinding = false;
    public AbilityUI rewindUI;
    [SerializeField] float rewindDuration = 5f;

    private Fire fireEffect;
    private float rewindTimer;
    private bool wasActive = false;

    void Start()
    {
        // Find Fire component in scene
        fireEffect = FindObjectOfType<Fire>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && rewindUI.IsReady)
        {
            IsRewinding = true;
            rewindTimer = rewindDuration;
            wasActive = true;
            if (fireEffect != null)
                fireEffect.StartRewind(rewindDuration);
        }

        if (IsRewinding && Input.GetKey(KeyCode.Q))
        {
            rewindTimer -= Time.deltaTime;
            rewindUI.DecreaseTimer(Time.deltaTime);
            if (fireEffect != null)
                fireEffect.UpdateTimer(Time.deltaTime);

            if (rewindTimer <= 0)
            {
                IsRewinding = false;
                rewindUI.TriggerCooldown();
                if (fireEffect != null)
                    fireEffect.StopRewind();
            }
        }
        else if (IsRewinding)
        {
            IsRewinding = false;
            if (wasActive)
            {
                rewindUI.TriggerCooldown();
                wasActive = false;
            }
            if (fireEffect != null)
                fireEffect.StopRewind();
        }
    }
}
