using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEntity : Entity
{
    [SerializeField]
    private float maxTimeToSleep = 2;

    private float birthTime;

    protected override void Awake()
    {
        base.Awake();

        birthTime = Time.time;
    }

    protected override void Update()
    {
        if (rb && !rb.isKinematic && !isFalling)
        {
            if (Time.time >= birthTime + maxTimeToSleep)
            {
                rb.isKinematic = true;
            }
        }
    }
}
