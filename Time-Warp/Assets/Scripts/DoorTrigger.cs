using System.Collections;
using UnityEngine;
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Transform doorChild;
    [SerializeField] float openHeight = 1f;
    [SerializeField] float openDuration = 5f;
    [SerializeField] float closeSpeed = 3f;

    Vector3 closedPosition;
    Vector3 openPosition;
    bool isOpening;
    bool isDoorOpen;

    public bool IsDoorOpen => isDoorOpen;

    void Start()
    {
        if (doorChild == null)
            doorChild = transform.GetChild(0);

        closedPosition = doorChild.localPosition;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOpening) return;
        if (!collision.collider.CompareTag("Player")) return;

        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        isOpening = true;

        // Move door up
        float elapsed = 0f;
        float duration = Vector3.Distance(closedPosition, openPosition) / 5f; // Approximate move time
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            doorChild.localPosition = Vector3.Lerp(closedPosition, openPosition, elapsed / duration);
            yield return null;
        }
        doorChild.localPosition = openPosition;
        isDoorOpen = true;

        // Wait for open duration
        yield return new WaitForSeconds(openDuration);

        // Fall down
        isDoorOpen = false;
        elapsed = 0f;
        while (doorChild.localPosition.y > closedPosition.y)
        {
            doorChild.localPosition -= Vector3.up * closeSpeed * Time.deltaTime;
            yield return null;
        }
        doorChild.localPosition = closedPosition;

        isOpening = false;
    }
}
