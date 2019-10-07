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

    [SerializeField]
    private float momentumDamping = 1;

    [SerializeField]
    private ParticleSystem burstParticles = default;

    private bool useMomentum = false;
    private Vector2 momentum;

    void Awake()
    {
        instance = this;
        Cursor = transform;
    }

    private void Update()
    {
        if (useMomentum)
        {
            var newPosition = Cursor.position + new Vector3(momentum.x, 0, momentum.y) * moveSpeed;

            newPosition.x = Mathf.Clamp(newPosition.x, World.Instance.Bounds.xMin, World.Instance.Bounds.xMax);
            newPosition.z = Mathf.Clamp(newPosition.z, World.Instance.Bounds.yMin, World.Instance.Bounds.yMax);

            Cursor.position = newPosition;

            momentum = Vector2.MoveTowards(momentum, Vector2.zero, Time.deltaTime * momentumDamping);
        }
        else if (momentum.sqrMagnitude > 0.1f)
        {
            useMomentum = true;
        }
    }

    public void Burst()
    {
        burstParticles?.Emit(50);
    }

    public void Move(Vector2 amount)
    {
        useMomentum = false;

        var newPosition = Cursor.position + new Vector3(amount.x, 0, amount.y) * moveSpeed;

        newPosition.x = Mathf.Clamp(newPosition.x, World.Instance.Bounds.xMin, World.Instance.Bounds.xMax);
        newPosition.z = Mathf.Clamp(newPosition.z, World.Instance.Bounds.yMin, World.Instance.Bounds.yMax);

        Cursor.position = newPosition;

        momentum = amount;
    }
}
