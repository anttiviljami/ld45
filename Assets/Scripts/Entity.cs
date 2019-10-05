using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public NoteSequence recipe;

    private Transform tr;

    [SerializeField]
    private float initialPositionJitter = 0.1f;

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
