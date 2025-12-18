using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] Vector3 fireOffset = new Vector3(0, 1.35f, 0);
    
    private Transform playerTransform;
    private GameObject fireInstance;
    private Animator fireAnimator;
    private bool hasPlayedConverge = false;
    private bool isActive = false;
    private float rewindTimer;
    private float slowmoTimer;
    private bool isSloummoMode = false;
    private const float convergeDuration = 10f / 12f; // 10 frames at 12 fps = 0.833 seconds

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
        if (!isActive) return;

        UpdateFirePosition();

        // Trigger converge animation when time remaining equals animation duration
        if (!hasPlayedConverge && ((!isSloummoMode && rewindTimer <= convergeDuration) || (isSloummoMode && slowmoTimer <= convergeDuration)))
        {
            PlayConvergeAnimation();
            hasPlayedConverge = true;
        }
    }

    public void StartRewind(float duration)
    {
        if (firePrefab == null || playerTransform == null) return;

        isActive = true;
        rewindTimer = duration;
        hasPlayedConverge = false;

        if (fireInstance == null)
        {
            Vector3 spawnPos = playerTransform.position + fireOffset;
            fireInstance = Instantiate(firePrefab, spawnPos, Quaternion.identity);
            fireAnimator = fireInstance.GetComponent<Animator>();
        }
    }

    public void StopRewind()
    {
        isActive = false;
        RemoveFire();
    }

    public void UpdateTimer(float deltaTime)
    {
        if (isActive)
            rewindTimer -= deltaTime;
    }

    public void StartSlowmo(float duration)
    {
        if (firePrefab == null || playerTransform == null) return;

        isActive = true;
        slowmoTimer = duration;
        isSloummoMode = true;
        hasPlayedConverge = false;

        if (fireInstance == null)
        {
            Vector3 spawnPos = playerTransform.position + fireOffset;
            fireInstance = Instantiate(firePrefab, spawnPos, Quaternion.identity);
            fireAnimator = fireInstance.GetComponent<Animator>();
        }
    }

    public void StopSlowmo()
    {
        isActive = false;
        isSloummoMode = false;
        RemoveFire();
    }

    public void UpdateSlowmoTimer(float deltaTime)
    {
        if (isActive && isSloummoMode)
            slowmoTimer -= deltaTime;
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
