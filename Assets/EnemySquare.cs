using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquare : EnemyBase
{

    private Quaternion originalRotation;
    protected override void Start()
    {
        base.Start();
        originalRotation = transform.rotation;
    }

    protected override void Update()
    {
        if (!target)
        {
            GetTarget();
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            MoveTowardsTarget();
            transform.rotation = originalRotation;
        }
    }

    private void MoveTowardsTarget() { 
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<IHealth>() is IHealth damagable)
            {
                damagable.TakeDamage(weaponDamage);
            }
        }
    }
}
