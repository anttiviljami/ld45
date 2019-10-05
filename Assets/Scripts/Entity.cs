using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public NoteSequence recipe;

    private Transform tr;

    [SerializeField]
    private float initialPositionJitter = 0.1f;

    private bool isFalling = false;

    void Awake()
    {
        tr = transform;
    }

    public Vector3 Jitter()
    {
        return new Vector3(
            Random.Range(-initialPositionJitter, initialPositionJitter),
            0,
            Random.Range(-initialPositionJitter, initialPositionJitter)
        );
    }

    private void Update()
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
            if (tr.position.y < -300)
            {
                Destroy(gameObject);
            }
        }
    }
}
