using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Entity
{
    [SerializeField]
    private float speed = 10;

    private bool isAlive = true;
    public bool IsAlive => isAlive && !isFalling;

    private Vector3 movement;
    private bool hasMovement = false;

    [SerializeField]
    private float hitPoints = 5;

    private float damage = 0;

    private float Health => Mathf.Clamp(hitPoints - damage, 0, hitPoints);

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private Transform flipSpriteWhenTurning;

    void FixedUpdate()
    {
        if (flipSpriteWhenTurning)
        {
            var scale = flipSpriteWhenTurning.localScale;
            if (scale.x > 0 && movement.x > 0.1)
            {
                scale.x = -Mathf.Abs(scale.x);
                flipSpriteWhenTurning.localScale = scale;
            }
            else if (scale.x < 0 && movement.x < -0.1)
            {
                scale.x = Mathf.Abs(scale.x);
                flipSpriteWhenTurning.localScale = scale;
            }
        }

        if (IsAlive && hasMovement)
        {
            ApplyMove();
        }
    }

    public void Move(Vector3 direction)
    {
        movement += direction;
        hasMovement = true;
    }

    private void ApplyMove()
    {
        var amount = movement.normalized * speed * Time.deltaTime;
        var newPosition = rb.position + new Vector3(amount.x, 0, amount.z) * speed;

        newPosition.x = Mathf.Clamp(newPosition.x, World.Instance.Bounds.xMin, World.Instance.Bounds.xMax);
        newPosition.y = 0;
        newPosition.z = Mathf.Clamp(newPosition.z, World.Instance.Bounds.yMin, World.Instance.Bounds.yMax);

        rb.position = newPosition;
        movement = Vector3.zero;
        hasMovement = false;
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;

        damage += amount;

        if (Health <= 0)
        {
            isAlive = false;
            if (deathEffect)
            {
                Instantiate(deathEffect, tr.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
