using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(EntityAttractBehaviour))]
public class HostileBehaviour : MonoBehaviour
{
    public static event System.Action<KillEventArgs> EntityKilled;

    private EntityAttractBehaviour attract;
    private LivingEntity entity;

    [SerializeField]
    private float coolOffAfterKill = 30;

    [SerializeField]
    private float damageInterval = 2;

    [SerializeField]
    private float damage = 1;

    [SerializeField]
    private float attackRange = 2;

    private float nextPossibleAttack;

    void Awake()
    {
        attract = GetComponent<EntityAttractBehaviour>();
        entity = GetComponent<LivingEntity>();
    }

    void Update()
    {
        if (Time.time >= nextPossibleAttack && attract.Target && attract.Target is LivingEntity)
        {
            if (Vector3.SqrMagnitude(entity.Position - attract.Target.Position) <= attackRange * attackRange)
            {
                Attack(attract.Target as LivingEntity);
            }
        }
    }

    public void Attack(LivingEntity target)
    {
        AttackEffect.Attack(entity.Position, target.Position);

        target.TakeDamage(damage);

        if (!target.IsAlive)
        {
            attract.moveStartLimit = Time.time + coolOffAfterKill;
            EntityKilled?.Invoke(new KillEventArgs() { killer = entity, victim = target });
        }

        nextPossibleAttack = Time.time + damageInterval;
    }
}
