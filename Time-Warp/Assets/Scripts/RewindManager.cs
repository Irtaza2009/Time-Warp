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
    public AudioClip rewindLoop;

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
            AudioManager.Instance.PlayLoop(rewindLoop, 0.3f);
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
                AudioManager.Instance.StopLoop();
                rewindUI.TriggerCooldown();
                if (fireEffect != null)
                    fireEffect.StopRewind();
            }
        }
        else if (IsRewinding)
        {
            IsRewinding = false;
            AudioManager.Instance.StopLoop();
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
