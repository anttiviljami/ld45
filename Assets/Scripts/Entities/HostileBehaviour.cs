using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(EntityAttractBehaviour))]
public class HostileBehaviour : MonoBehaviour
{
    private EntityAttractBehaviour attract;
    private LivingEntity entity;

    [SerializeField]
    private float coolOffAfterKill = 30;
    private float nextPossibleChase;

    [SerializeField]
    private float damageInterval = 2;

    [SerializeField]
    private float damage = 1;

    [SerializeField]
    private float attackRange = 2;

    void Awake()
    {
        attract = GetComponent<EntityAttractBehaviour>();
        entity = GetComponent<LivingEntity>();
    }

    void Update()
    {
        if (attract.Target && attract.Target is LivingEntity)
        {
            if (Vector3.SqrMagnitude(entity.Position - attract.Target.Position) <= attackRange * attackRange)
            {
                Attack(attract.Target as LivingEntity);
            }
        }
    }

    public void Attack(LivingEntity target)
    {
        target.TakeDamage(damage);
        if (!target.IsAlive)
        {
            nextPossibleChase = Time.time + coolOffAfterKill;
        }
        attract.moveStartLimit = Time.time + damageInterval;
    }
}
