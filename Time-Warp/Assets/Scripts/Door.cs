using UnityEngine;
using UnityEngine.SceneManagement;


public class Door : MonoBehaviour
{
    [SerializeField] DoorTrigger doorTrigger; 
    [SerializeField] string playerTag = "Player";
    public AudioClip doorClip;

    void Awake()
    {
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (doorTrigger == null) return;

        if (doorTrigger.IsDoorOpen)
        {
            Scene current = SceneManager.GetActiveScene();
            AudioManager.Instance.PlaySFX(doorClip, 0.7f);
            SceneManager.LoadScene(current.buildIndex + 1);
        }
    }
}
