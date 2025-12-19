using UnityEngine;
using System.Collections;

public class ButtonDelay : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    [SerializeField] float fadeInDuration = 0.5f;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvasGroup != null)
        {
            // Start invisible
            canvasGroup.alpha = 0f;
            StartCoroutine(FadeInAfterDelay());
        }
    }

    IEnumerator FadeInAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
