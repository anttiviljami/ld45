using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Transform tr;

    private float initialPositionJitter = 0.1f;

    [SerializeField]
    private float clearSpaceSpeedFactor = 1;

    private const float defaultClearSpaceSpeed = 10;

    void Awake()
    {
        tr = transform;
    }

    public Vector3 Jitter()
    {
        return new Vector3(
            Random.Range(-initialPositionJitter, initialPositionJitter),
            0,
            Random.Range(-initialPositionJitter, initialPositionJitter)
        );
    }

    // TODO fall of edge
}
