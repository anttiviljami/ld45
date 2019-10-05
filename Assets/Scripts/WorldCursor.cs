using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCursor : MonoBehaviour
{
    private static WorldCursor instance;
    public static WorldCursor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WorldCursor>();
            }

            return instance;
        }
    }

    public Transform Cursor { get; private set; }

    [SerializeField]
    private float moveSpeed = 0.01f;

    void Awake()
    {
        instance = this;
        Cursor = transform;
    }

    public void Move(Vector2 amount)
    {
        Cursor.position += new Vector3(amount.x, 0, amount.y) * moveSpeed;
        // TODO bounds
    }
}
