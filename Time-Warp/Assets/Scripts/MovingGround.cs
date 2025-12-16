using UnityEngine;
using System.Collections;

public class MovingGround : MonoBehaviour
{
    [SerializeField] float moveDelay = 1f;
    [SerializeField] float moveSpeed = 5f;

    float moveDistance;
    bool isMoving;
    bool hasFinished;
    Coroutine moveRoutine;
    bool wasRewinding;

    void Start()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            moveDistance = col.bounds.size.x;
        }
        else
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                moveDistance = sr.bounds.size.x;
            }
            else
            {
                moveDistance = 5f; 
            }
        }

        moveRoutine = StartCoroutine(StartAndMove());
    }

    void Update()
    {
        if (wasRewinding && !RewindManager.IsRewinding)
        {
            ResumeAfterRewind();
        }
        wasRewinding = RewindManager.IsRewinding;
    }

    IEnumerator StartAndMove()
    {
        isMoving = false;
        yield return new WaitForSeconds(moveDelay);
        yield return MoveLeftByDistance(moveDistance);
        hasFinished = true;
        moveRoutine = null;
    }

    IEnumerator MoveLeftByDistance(float distance)
    {
        isMoving = true;
        float moved = 0f;
        Vector3 dir = Vector3.left;
        while (moved < distance)
        {
            float step = moveSpeed * Time.deltaTime;
            if (moved + step > distance) step = distance - moved;
            transform.position += dir * step;
            moved += step;
            yield return null;
        }
        isMoving = false;
    }

    public void ResumeAfterRewind()
    {
        if (hasFinished) return;

        if (isMoving) return;

        if (moveRoutine == null)
        {
            float remaining = ComputeRemainingDistance();
            moveRoutine = StartCoroutine(MoveLeftByDistance(Mathf.Max(0f, remaining)));
        }
    }

    float ComputeRemainingDistance()
    {
        var col = GetComponent<Collider2D>();
        var sr = GetComponent<SpriteRenderer>();
        float width = col != null ? col.bounds.size.x : (sr != null ? sr.bounds.size.x : moveDistance);
        
        return width;
    }
}
