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
    private Rect bounds = new Rect(0, 0, 100, 100);

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

        newPosition.x = Mathf.Clamp(newPosition.x, -bounds.size.x * 0.5f, bounds.size.x * 0.5f);
        newPosition.y = Mathf.Clamp(newPosition.y, -bounds.size.y * 0.5f, bounds.size.y * 0.5f);

        Cursor.position = newPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var size = new Vector3(bounds.size.x, 10, bounds.size.y);
        Gizmos.DrawWireCube(Vector3.up * 5, size);
    }
}
