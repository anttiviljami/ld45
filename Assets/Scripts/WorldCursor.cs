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
    private float idleTime = 25;

    [SerializeField]
    private ParticleSystem burstParticles = default;

    private bool useMomentum = false;
    private Vector2 momentum;

    private float lastMovedTime;

    private LTDescr moveTween;

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
        else if (Time.time > lastMovedTime + idleTime)
        {
            var targetPosition = new Vector3(
                Random.Range(World.Instance.Bounds.xMin * 0.8f, World.Instance.Bounds.xMax * 0.8f),
                0,
                Random.Range(World.Instance.Bounds.yMin * 0.8f, World.Instance.Bounds.yMax * 0.8f));
            moveTween = LeanTween
                .move(gameObject, targetPosition, 1.3f)
                .setEaseInOutCirc();
            lastMovedTime = Time.time;
        }
    }

    public void Burst()
    {
        burstParticles?.Emit(50);
    }

    public void Move(Vector2 amount)
    {
        if (moveTween != null)
        {
            LeanTween.cancel(moveTween.id);
            moveTween = null;
        }

        useMomentum = false;

        var newPosition = Cursor.position + new Vector3(amount.x, 0, amount.y) * moveSpeed;

        newPosition.x = Mathf.Clamp(newPosition.x, World.Instance.Bounds.xMin, World.Instance.Bounds.xMax);
        newPosition.z = Mathf.Clamp(newPosition.z, World.Instance.Bounds.yMin, World.Instance.Bounds.yMax);

        Cursor.position = newPosition;

        momentum = amount;

        lastMovedTime = Time.time;
    }
}
