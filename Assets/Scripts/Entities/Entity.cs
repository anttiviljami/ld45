using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public static event System.Action<Entity> EntitySpawned;
    public static event System.Action<Entity> EntityRemoved;

    public NoteSequence recipe;

    protected Transform tr;

    [SerializeField]
    private float initialPositionJitter = 0.1f;

    protected bool isFalling = false;

    protected Rigidbody rb;

    private static readonly Vector3 BOUNDS_SIZE_DEFAULT = Vector3.one * 10;

    public NoteSequence NoteSequence { get; private set; }

    public Vector3 Position => tr.position;

    [SerializeField]
    private Vector3 spawnOffset = Vector3.zero;

    protected virtual void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        EntitySpawned?.Invoke(this);
    }

    void OnDisable()
    {
        EntityRemoved?.Invoke(this);
    }

    public void InitializeWithNoteSequence(NoteSequence sequence)
    {
        NoteSequence = sequence;
    }

    public Vector3 JitteredPosition(Vector3 position)
    {
        var col = GetComponent<Collider>();
        var size = col && !col.bounds.Equals(default(Bounds)) ? col.bounds.size * 1.05f : BOUNDS_SIZE_DEFAULT;

        position.x += Random.Range(-initialPositionJitter, initialPositionJitter);
        position.z += Random.Range(-initialPositionJitter, initialPositionJitter);

        position.x = Mathf.Clamp(position.x, World.Instance.Bounds.xMin + size.x * 0.5f, World.Instance.Bounds.xMax - size.x * 0.5f);
        position.z = Mathf.Clamp(position.z, World.Instance.Bounds.yMin + size.z * 0.5f, World.Instance.Bounds.yMax - size.z * 0.5f);

        return position + spawnOffset;
    }

    protected virtual void Update()
    {
        if (rb)
        {
            if (!isFalling && !World.Instance.Bounds.Contains(new Vector2(tr.position.x, tr.position.z)))
            {
                isFalling = true;
                var rb = GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.drag = Mathf.Clamp(rb.drag * 0.5f, 0, 0.25f);
                rb.detectCollisions = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                rb.AddTorque(Vector3.forward * Random.Range(-10, 10));
            }
            else
            {
                if (tr.position.y < -100)
                {
                    AchievementsController.Instance.TriggerAchivement(AchievementsController.Achievement.OffTheEdge);
                    Destroy(gameObject);
                }
            }
        }
    }
}
