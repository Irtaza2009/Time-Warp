using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
