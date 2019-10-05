﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    private static World instance;
    public static World Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<World>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Vector2 size = new Vector2(100, 100);

    public Vector2 Size => size;

    void Awake()
    {
        instance = this;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var size3d = new Vector3(size.x, 10, size.y);
        Gizmos.DrawWireCube(Vector3.up * 5, size3d);
    }
}