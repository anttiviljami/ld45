using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    private class AttackEffectManager
    {
        private static AttackEffectManager instance;
        public static AttackEffectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AttackEffectManager();
                }
                return instance;
            }
        }

        private static Queue<AttackEffect> attackPool;

        private AttackEffect prefab;
        private AttackEffectManager()
        {
            attackPool = new Queue<AttackEffect>();
            prefab = Resources.Load<AttackEffect>("AttackEffect");
        }

        public AttackEffect GetEffect()
        {
            AttackEffect attack = null;
            if (attackPool.Count > 0)
            {
                attack = attackPool.Dequeue();
                attack.gameObject.SetActive(true);
            }
            else
            {
                attack = Instantiate(prefab);
            }

            return attack;
        }

        public static void ReleaseEffect(AttackEffect effect)
        {
            if (instance != null)
            {
                instance.ReleaseEffect_Instance(effect);
            }
        }

        public void ReleaseEffect_Instance(AttackEffect effect)
        {
            attackPool.Enqueue(effect);
        }
    }

    public static void Attack(Vector3 attackerPosition, Vector3 targetPosition)
    {
        AttackEffect attack = AttackEffectManager.Instance.GetEffect();

        attack.ApplyAttack(attackerPosition, targetPosition);
    }

    [SerializeField]
    private ParticleSystem particles = default;

    private Transform tr;

    public void ApplyAttack(Vector3 attackerPosition, Vector3 targetPosition)
    {
        if (!tr) tr = transform;

        tr.position = attackerPosition;
        tr.LookAt(targetPosition);

        particles.Play();
    }

    void Update()
    {
        if (!particles.IsAlive())
        {
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        AttackEffectManager.ReleaseEffect(this);
    }
}
