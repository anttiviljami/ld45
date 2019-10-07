using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class MillAboutBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool checkForEdges = true;

    private Transform tr;

    private Vector3 targetPosition;

    private LivingEntity entity;

    public bool CanMillAbout { get; set; }

    private const int MOVE_PRIORITY = 10;

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
                entity.Move(targetPosition - tr.position, MOVE_PRIORITY);
            }
        }
    }

    void GetNewTarget()
    {
        nextTargetTime = Time.time + Random.Range(10, 30);

        if (checkForEdges)
        {
            targetPosition = new Vector3(
                Random.Range(World.Instance.Bounds.xMin, World.Instance.Bounds.xMax),
                0,
                Random.Range(World.Instance.Bounds.yMin, World.Instance.Bounds.yMax));
        }
        else
        {
            targetPosition = tr.position + new Vector3(
                Random.Range(-50, 50),
                0,
                Random.Range(-50, 50));
        }
    }
}
