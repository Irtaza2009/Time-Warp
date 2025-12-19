using UnityEngine;
using TMPro;
using System.Collections;

public class TextDelay : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    [SerializeField] float fadeInDuration = 0.5f;

    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        
        if (textComponent != null)
        {
            // Start invisible
            Color c = textComponent.color;
            c.a = 0f;
            textComponent.color = c;

            StartCoroutine(FadeInAfterDelay());
        }
    }

    IEnumerator FadeInAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        Color c = textComponent.color;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            textComponent.color = c;
            yield return null;
        }

        c.a = 1f;
        textComponent.color = c;
    }
}
