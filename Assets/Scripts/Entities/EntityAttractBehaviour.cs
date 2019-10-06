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

    private Entity target;

    private float nextMoveStart;
    private float moveEndTime;

    private MillAboutBehaviour milling;

    void Awake()
    {
        entity = GetComponent<LivingEntity>();
        milling = GetComponent<MillAboutBehaviour>();
    }

    void Update()
    {
        if (target)
        {
            entity.Move(target.Position - entity.Position);
            if (Time.time > moveEndTime)
            {
                if (milling) milling.CanMillAbout = true;
                target = null;
                nextMoveStart = Time.time + moveInterval;
            }
        }
        else if (Time.time > nextMoveStart)
        {
            target = EntityManager.Instance.GetClosestInRange(entity.Position, range, targetNote1, targetNote2);
            if (target)
            {
                if (milling) milling.CanMillAbout = false;
                moveEndTime = Time.time + moveTime;
            }
        }
    }
}
