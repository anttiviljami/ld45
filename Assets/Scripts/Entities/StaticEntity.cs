using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEntity : Entity
{
    [SerializeField]
    private float maxTimeToSleep = 10;

    [SerializeField]
    private float maxMassFactor = 10;

    [SerializeField]
    private bool becomeStatic = false;

    private float birthTime;

    private float targetMass;

    private float massIncreaseSpeed;

    protected override void Awake()
    {
        base.Awake();

        birthTime = Time.time;

        targetMass = rb.mass * maxMassFactor;

        massIncreaseSpeed = (targetMass - rb.mass) / maxTimeToSleep;
    }

    protected override void Update()
    {
        if (rb && !rb.isKinematic && !isFalling)
        {
            if (Time.time <= birthTime + maxTimeToSleep && rb.mass < targetMass)
            {
                rb.mass = Mathf.MoveTowards(rb.mass, targetMass, Time.deltaTime * massIncreaseSpeed);

                if (rb.mass >= targetMass && becomeStatic)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }
}
