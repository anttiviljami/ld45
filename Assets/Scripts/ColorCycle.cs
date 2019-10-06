using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color color1 = Color.yellow;

    [SerializeField]
    private Color color2 = Color.red;

    [SerializeField]
    private float speed = 3;

    void Update()
    {
        spriteRenderer.color = Color.Lerp(color1, color2, 0.5f * Mathf.Sin(Time.time * speed) + 0.5f);
    }
}
