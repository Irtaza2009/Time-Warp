using UnityEngine;

public class GhostFade : MonoBehaviour
{
    private SpriteRenderer sr;
    public float fadeSpeed = 2f;
    [SerializeField] float startAlpha = 0.5f;
    [SerializeField] Color timeTint = new Color(0.3f, 0.8f, 1f, 1f); // Cyan tint

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetOriginalColor(Color originalColor)
    {
        // Blend original color with time tint and set reduced opacity
        Color blendedColor = Color.Lerp(originalColor, timeTint, 0.4f);
        blendedColor.a = startAlpha;
        sr.color = blendedColor;
    }

    void Update()
    {
        Color c = sr.color;
        c.a -= fadeSpeed * Time.deltaTime;
        sr.color = c;

        if (c.a <= 0f)
            Destroy(gameObject);
    }
}
