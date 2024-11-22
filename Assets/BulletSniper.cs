using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSniper : BulletBase
{

    protected override void Start()
    {
        base.Start();
    }

    public void Initialize(GameObject creator)
    {
        this.creator = creator;
        Collider2D bulletCollider = GetComponent<Collider2D>();
        Collider2D creatorCollider = creator.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(bulletCollider, creatorCollider, true);
        StartCoroutine(EnableCollisionWithCreator(bulletCollider, creatorCollider));
    }
    private IEnumerator EnableCollisionWithCreator(Collider2D bulletCollider, Collider2D creatorCollider)
    {
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(bulletCollider, creatorCollider, false);
        Physics2D.GetIgnoreCollision(bulletCollider, creatorCollider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "indistructableWall" when bounceCount < maxBounce:


                // vietoj bounceCount++ panaudoti bitines operacijas
                int increment = 1;
                while (increment != 0)
                {
                    int carry = bounceCount & increment;  // Calculate the carry
                    bounceCount = bounceCount ^ increment;      // XOR to add without the carry
                    increment = carry << 1;         // Shift the carry to the left
                }


                Vector2 reflectDir = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
                rb.velocity = reflectDir.normalized * speed;
                transform.up = reflectDir;

                if (Mathf.Abs(Vector2.Dot(reflectDir, collision.contacts[0].normal)) < 0.1f)
                {
                    reflectDir = Quaternion.Euler(0, 0, 10) * reflectDir;
                    rb.velocity = reflectDir.normalized * speed;
                    transform.up = reflectDir;
                }
                break;

            case "indistructableWall":
                Destroy(gameObject);
                break;

            case "Bullet":
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;

            default:
                IHealth damagable = collision.gameObject.GetComponent<IHealth>();
                if (damagable != null)
                {
                    damagable.TakeDamage(bulletDamage);
                    Destroy(gameObject);
                }
                break;
        }
    }
}