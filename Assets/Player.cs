using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MonoBehaviour, IHealth, IFormattable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int health = 10;
    
    [SerializeField] private GameObject bulletRiflePrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] public int maxAmmo = 10;
    public int currentAmmo;
    [SerializeField] public float reloadTime = 2f;
    private bool isReloading = false;

    public int killCount = 0;

    [SerializeField] private float teleportDistance = 5f;
    [SerializeField] private float teleportCooldown = 2f;


    private Rigidbody2D rb;
    private float mx;
    private float my;

    private float fireTimer;
    private float teleportTimer;

    private Vector2 mousePos;

    private GenericHealthComponent<Player> healthComponent;

    public event Action<int> PlayerDamaged;
    public event Action<int> PlayerShot;
    public event Action<bool> PlayerReloading;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthComponent = new GenericHealthComponent<Player>(this);
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //health.PrintDebug("Health: ");

        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x -
            transform.position.x) * Mathf.Rad2Deg - 90f;

        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0) && fireTimer <= 0f && currentAmmo > 0)
        {
            Shoot();
            fireTimer = fireRate;
        } else
        {
            fireTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && teleportTimer <= 0f)
        {
            Teleport();
            teleportTimer = teleportCooldown;
        }
        else
        {
            teleportTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading) {
            StartCoroutine(Reload()); 
        }

        //Debug.Log(this.ToString("G", null));

    }

    private void FixedUpdate() => rb.velocity = new Vector2(mx, my).normalized * speed;

    IEnumerator Reload() { 
        isReloading = true;
        currentAmmo = 0;
        PlayerReloading?.Invoke(true);
        PlayerShot?.Invoke(0);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo; 
        isReloading = false;
        PlayerReloading?.Invoke(false);
        PlayerShot?.Invoke(maxAmmo);
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletRiflePrefab, firingPoint.position, firingPoint.rotation);
        bullet.GetComponent<BulletRifle>().Initialize(gameObject);
        currentAmmo--;
        PlayerShot?.Invoke(currentAmmo);
    }

    private void Teleport()
    {
        Vector2 direction = new Vector2(mx, my).normalized;
        rb.position += direction * teleportDistance;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        PlayerDamaged?.Invoke(health);
        if (health <= 0)
        {
            Destroy(gameObject);
            LevelManager.manager.GameOver();
        }
    }

    public void Damage(int takeDamageAmount)
    {
        healthComponent.ApplyDamage(takeDamageAmount);
    }

    public void Heal(int amount)
    {
        Health += amount;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";
        if (formatProvider == null) formatProvider = System.Globalization.CultureInfo.CurrentCulture;

        switch (format.ToUpperInvariant())
        {
            case "G":
                return $"Player: CurrentVelocity={rb.velocity}, Health={health}, FireRateTimer={fireTimer}, TeleportTimer={teleportTimer}";
            case "H":
                return $"Health={health}";
            default:
                throw new FormatException($"The {format} format string is not supported.");
        }
    }

    public override string ToString()
    {
        return ToString("G", null);
    }

}

