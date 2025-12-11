using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [Header("Cooldown Settings")]
    public Image cooldownFill;     // the fill image
    public float cooldownTime = 3f;
    float cooldownTimer = 0f;

    public bool IsReady => cooldownTimer <= 0;

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Fill amount goes from 0 (on cooldown) to 1 (ready)
        if (cooldownTimer > 0)
        {
            cooldownFill.fillAmount = 1f - (cooldownTimer / cooldownTime);
        }
        else
        {
            cooldownFill.fillAmount = 1f;
        }
    }

    public void TriggerCooldown()
    {
        cooldownTimer = cooldownTime;
    }

    public void DecreaseTimer(float amount)
    {
        cooldownTimer -= amount;
    }
}
