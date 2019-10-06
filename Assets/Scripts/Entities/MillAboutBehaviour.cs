using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class MillAboutBehaviour : MonoBehaviour
{
    private Transform tr;

    private Vector3 targetPosition;

    private LivingEntity entity;

    public bool CanMillAbout { get; set; }

    private float nextTargetTime = 0;

    void Awake()
    {
        entity = GetComponent<LivingEntity>();
        tr = GetComponent<Transform>();
        CanMillAbout = true;
    }

    void Update()
    {
        if (CanMillAbout && entity.IsAlive)
        {
            if (Time.time >= nextTargetTime)
            {
                GetNewTarget();
            }
            else
            {
                entity.Move(targetPosition - tr.position);
            }
        }
    }

    void GetNewTarget()
    {
        nextTargetTime = Time.time + Random.Range(10, 30);
        targetPosition = new Vector3(
            Random.Range(World.Instance.Bounds.xMin, World.Instance.Bounds.xMax),
            0,
            Random.Range(World.Instance.Bounds.yMin, World.Instance.Bounds.yMax)
        );
    }
}
