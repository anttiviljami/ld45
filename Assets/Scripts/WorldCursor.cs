using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCursor : MonoBehaviour
{
    public Transform Cursor { get; private set; }

    void Awake()
    {
        Cursor = transform;

        // TODO bounds
    }
}
