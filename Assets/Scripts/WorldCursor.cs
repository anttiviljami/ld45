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

    [SerializeField]
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
        var newPosition = Cursor.position + new Vector3(amount.x, 0, amount.y) * moveSpeed;

        newPosition.x = Mathf.Clamp(newPosition.x, World.Instance.Bounds.xMin, World.Instance.Bounds.xMax);
        newPosition.z = Mathf.Clamp(newPosition.z, World.Instance.Bounds.yMin, World.Instance.Bounds.yMax);

        Cursor.position = newPosition;
    }
}
