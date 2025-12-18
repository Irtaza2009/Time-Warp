using UnityEngine;
using System.Collections.Generic;

public class RewindManager : MonoBehaviour
{
    public static bool IsRewinding = false;
    public AbilityUI rewindUI;
    [SerializeField] float rewindDuration = 5f;
    [SerializeField] GameObject firePrefab;
    [SerializeField] Vector3 fireOffset = new Vector3(0, 1f, 0);
    
    private float rewindTimer;
    private bool wasActive = false;
    private Transform playerTransform;
    private GameObject fireInstance;
    private Animator fireAnimator;
    private bool hasPlayedConverge = false;
    private const float convergeDuration = 10f / 12f; // 10 frames at 12 fps = 0.833 seconds

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && rewindUI.IsReady)
        {
            IsRewinding = true;
            rewindTimer = rewindDuration;
            wasActive = true;
            SpawnFire();
        }

        if (IsRewinding && Input.GetKey(KeyCode.Q))
        {
            rewindTimer -= Time.deltaTime;
            rewindUI.DecreaseTimer(Time.deltaTime);
            UpdateFirePosition();

            // Trigger converge animation when time remaining equals animation duration
            if (!hasPlayedConverge && rewindTimer <= convergeDuration)
            {
                PlayConvergeAnimation();
                hasPlayedConverge = true;
            }

            if (rewindTimer <= 0)
            {
                IsRewinding = false;
                rewindUI.TriggerCooldown();
                RemoveFire();
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
            RemoveFire();
        }
    }

    void SpawnFire()
    {
        if (firePrefab == null || playerTransform == null) return;
        if (fireInstance != null) return;

        Vector3 spawnPos = playerTransform.position + fireOffset;
        fireInstance = Instantiate(firePrefab, spawnPos, Quaternion.identity);
        fireAnimator = fireInstance.GetComponent<Animator>();
        hasPlayedConverge = false;
    }

    void PlayConvergeAnimation()
    {
        if (fireAnimator != null)
        {
            fireAnimator.Play("Fire_Converge");
        }
    }

    void UpdateFirePosition()
    {
        if (fireInstance != null && playerTransform != null)
        {
            fireInstance.transform.position = playerTransform.position + fireOffset;
        }
    }

    void RemoveFire()
    {
        if (fireInstance != null)
        {
            Destroy(fireInstance);
            fireInstance = null;
            fireAnimator = null;
        }
    }
}
