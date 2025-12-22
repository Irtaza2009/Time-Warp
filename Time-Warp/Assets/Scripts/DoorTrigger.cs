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
    private Coroutine openCoroutine;

    public bool IsDoorOpen => isDoorOpen;
    public bool IsOpening => isOpening;
    public Vector3 DoorChildPosition => doorChild.localPosition;
    public AudioClip doorTriggerClip;

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

        openCoroutine = StartCoroutine(OpenDoor());
        AudioManager.Instance.PlaySFX(doorTriggerClip, 0.7f);
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
        openCoroutine = null;
    }

    public void RestoreStateFromRewind(bool open, bool opening, Vector3 childPosition)
    {
        // Stop the current coroutine if running
        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);
            openCoroutine = null;
        }

        isDoorOpen = open;
        isOpening = opening;
        doorChild.localPosition = childPosition;
    }

    public void ResumeAfterRewind()
    {
        // If door was opening, restart the open sequence
        if (isOpening)
        {
            openCoroutine = StartCoroutine(OpenDoor());
        }
        // If door was open, start the close sequence
        else if (isDoorOpen)
        {
            openCoroutine = StartCoroutine(CloseDoorAfterDuration());
        }
    }

    IEnumerator CloseDoorAfterDuration()
    {
        // Wait for open duration
        yield return new WaitForSeconds(openDuration);

        // Fall down
        isDoorOpen = false;
        float elapsed = 0f;
        while (doorChild.localPosition.y > closedPosition.y)
        {
            doorChild.localPosition -= Vector3.up * closeSpeed * Time.deltaTime;
            yield return null;
        }
        doorChild.localPosition = closedPosition;

        isOpening = false;
        openCoroutine = null;
    }
}
