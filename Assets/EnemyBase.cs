using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IHealth, ICloneable
{
    [SerializeField] public int health;
    public float speed = 3f;
    public float rotationSpeed = 0.0025f;
    [SerializeField] public int weaponDamage;

    protected Rigidbody2D rb;
    public Transform target;

    protected GenericHealthComponent<EnemyBase> healthComponent;

    public event Action<int> EnemyDamaged;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    protected virtual void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        healthComponent = new GenericHealthComponent<EnemyBase>(this);
    }

    protected virtual void Update()
    {

    }

    public void Damage(int takeDamageAmount)
    {
        healthComponent.ApplyDamage(takeDamageAmount);
    }
    public virtual void TakeDamage(int damage)
    {   
        health -= damage;
        EnemyDamaged?.Invoke(health);
        if (health <= 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        Health += amount;
    }

    protected virtual void Die()
    {
        FindObjectOfType<EnemyManager>().UnregisterEnemy(this);
        Destroy(gameObject);
        
    }

    protected void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }


}
