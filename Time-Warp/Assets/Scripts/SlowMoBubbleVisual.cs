using UnityEngine;

public class SlowMoBubbleVisual : MonoBehaviour
{
    public Transform player;
    public SpriteRenderer bubbleSprite;
    public float maxScale = 4f;

    void Update()
    {
        transform.position = player.position;
        float s = SlowMoBubble.Instance.isActive ? SlowMoBubble.Instance.radius : 0f;
        bubbleSprite.transform.localScale = new Vector3(s, s, 1);
        bubbleSprite.color = SlowMoBubble.Instance.isActive ? new Color(0, 0.6f, 1f, 0.3f) : Color.clear;
    }
}
