using UnityEngine;
using System.Collections.Generic;

public class RewindManager : MonoBehaviour
{
    public static bool IsRewinding = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            IsRewinding = true;

        if (Input.GetKeyUp(KeyCode.Q))
            IsRewinding = false;
    }
}
