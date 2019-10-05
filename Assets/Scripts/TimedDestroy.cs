using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 3;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
