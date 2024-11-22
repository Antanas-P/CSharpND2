using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<EnemyBase> enemies = new List<EnemyBase>();

    public IEnumerable<EnemyBase> Enemies => enemies;

    public event Action EnemyListUpdated;
    public void RegisterEnemy(EnemyBase enemy)
    {
        enemies.Add(enemy);
        enemy.EnemyDamaged += OnEnemyDamaged;
        EnemyListUpdated?.Invoke();
    }

    public void UnregisterEnemy(EnemyBase enemy)
    {
        enemies.Remove(enemy);
        enemy.EnemyDamaged -= OnEnemyDamaged;
        EnemyListUpdated?.Invoke();
    }

    private void OnEnemyDamaged(int health)
    {
        EnemyListUpdated?.Invoke();
    }

    public IEnumerable<EnemyBase> GetActiveEnemiesLINQ()
    {
        return enemies.Where(enemy => enemy != null && enemy.gameObject.activeInHierarchy);
    }

    public IEnumerable<EnemyBase> GetActiveEnemiesIterator()
    {
        foreach (var enemy in enemies)
        { 
            yield return enemy; 
        }
    }

}
