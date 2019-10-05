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

        newPosition.x = Mathf.Clamp(newPosition.x, -World.Instance.Size.x * 0.5f, World.Instance.Size.x * 0.5f);
        newPosition.y = Mathf.Clamp(newPosition.y, -World.Instance.Size.y * 0.5f, World.Instance.Size.y * 0.5f);

        Cursor.position = newPosition;
    }
}
