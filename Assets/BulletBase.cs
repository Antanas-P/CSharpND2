using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [Range(1, 30)]
    [SerializeField] protected float speed = 10f;

    [Range(1, 10)]
    [SerializeField] protected float lifeTime = 3f;

    protected Rigidbody2D rb;

    [SerializeField] protected int bulletDamage = 1;
    protected int bounceCount = 0;
    protected int maxBounce = 1;

    protected GameObject creator;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    protected virtual void FixedUpdate() => rb.velocity = transform.up * speed;

}
