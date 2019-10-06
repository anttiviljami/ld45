using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class EntityAttractBehaviour : MonoBehaviour
{
    [SerializeField]
    private float moveTime = 20;

    [SerializeField]
    private float moveInterval = 20;

    [SerializeField]
    private Note.Name targetNote1;

    [SerializeField]
    private Note.Name targetNote2;

    [SerializeField]
    private float range = 20;

    private LivingEntity entity;

    public Entity Target { get; private set; }

    private float nextMoveStart;
    private float moveEndTime;

    // Do not allow new movement until this timestamp
    public float moveStartLimit;

    private const int MOVE_PRIORITY = 20;

    void Awake()
    {
        entity = GetComponent<LivingEntity>();
    }

    void Update()
    {
        if (Target)
        {
            entity.Move(Target.Position - entity.Position, MOVE_PRIORITY);
            if (Time.time > moveEndTime)
            {
                Target = null;
                nextMoveStart = Time.time + moveInterval;
            }
        }
        else if (Time.time > nextMoveStart && Time.time > moveStartLimit)
        {
            Target = EntityManager.Instance.GetClosestInRange(entity.Position, range, targetNote1, targetNote2, entity.recipe);
            if (Target)
            {
                moveEndTime = Time.time + moveTime;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(entity.Position, Target.Position);
        }
    }
}
