using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    // Player
    public Player player;

    // Managers
    private EnemyManager enemyManager;

    // UI elements
    public TMPro.TextMeshProUGUI enemyList;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI ammoText;
    public TMPro.TextMeshProUGUI reloadStatusText;

    private void Start()
    {
        try
        {
            enemyManager = FindObjectOfType<EnemyManager>();
            if (enemyManager == null)
            {
                throw new Exception("EnemyManager not found");
            }

            if (player == null)
            {
                player = FindObjectOfType<Player>();
                if (player == null)
                {
                    throw new Exception("Player object not found");
                }
            }

            
            player.PlayerDamaged += OnPlayerDamaged;
            player.PlayerShot += OnPlayerShot;
            player.PlayerReloading += OnPlayerReload;

            
            OnPlayerDamaged(player.Health);
            OnPlayerShot(player.maxAmmo);

            
            enemyManager.EnemyListUpdated += UpdateEnemyListUI;

            
            UpdateEnemyListUI();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in UIManager Start: {ex.Message}");
        }
    }

    void UpdateEnemyListUI()
    {
        IEnumerator<EnemyBase> enumerator = enemyManager.GetActiveEnemiesLINQ().GetEnumerator();

        string enemyListString = "Targets:\n";
        while (enumerator.MoveNext())
        {
            var enemy = enumerator.Current;
            enemyListString += $"{enemy.name} - Health: {enemy.Health}\n";
        }

        enemyList.text = enemyListString;
    }

    private void OnEnable()
    {
        if (player != null)
        {
            player.PlayerDamaged += OnPlayerDamaged;
            player.PlayerShot += OnPlayerShot;
            player.PlayerReloading += OnPlayerReload;
        }

        if(enemyManager != null)
        {
            enemyManager.EnemyListUpdated += UpdateEnemyListUI;
        }
        
    }
    private void OnDisable() 
    { 
        player.PlayerDamaged -= OnPlayerDamaged;
        player.PlayerShot -= OnPlayerShot;
        player.PlayerReloading -= OnPlayerReload;
        enemyManager.EnemyListUpdated -= UpdateEnemyListUI;
    }
    private void OnPlayerDamaged(int currentHealth)
    { 
        healthText.text = $"Health: {currentHealth}"; 
    }    
    private void OnPlayerShot(int currentAmmo)
    { 
        ammoText.text = $"Ammo: {currentAmmo}"; 
    }
    private void OnPlayerReload(bool status)
    {
        reloadStatusText.text = status ? $"Reloading..." : $"";

    }

}
