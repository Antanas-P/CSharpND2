using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    // Managers
    public static LevelManager manager;
    private EnemyManager enemyManager;

    // Menu, UI
    public GameObject deathScreen;


    // Enemy prefabs
    public GameObject enemySquarePrefab;
    public GameObject enemyTrianglePrefab;

    // Priesu atsiradimo koordinates
    public List<Vector3> enemySquarePositions = new List<Vector3>();
    public List<Vector3> enemyTrianglePositions = new List<Vector3>();

    private void Awake()
    {
        manager = this;
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Start()
    {
        PopulateSpawnPositions();
        SpawnEnemies();
    }

    public void GameOver()
    {
        deathScreen.SetActive(true);
    }


    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PopulateSpawnPositions()
    {
        // Uplidome priesu atsiradimo koordinates naudojant LINQ
        enemySquarePositions = Enumerable.Range(0, 3).Select(i => new Vector3(i * 1.2f, 1, 0)).ToList();
        enemyTrianglePositions = Enumerable.Range(0, 5).Select(i => new Vector3(i * 1.3f, i * -1.3f, 0)).ToList();
    }

    void SpawnEnemies()
    {

        enemySquarePositions.ForEach(position =>
        {
            var enemy = Instantiate(enemySquarePrefab, position, Quaternion.identity).GetComponent<EnemyBase>();
            enemyManager.RegisterEnemy(enemy);
        });


        enemyTrianglePositions.ForEach(position =>
        {
            var enemy = Instantiate(enemyTrianglePrefab, position, Quaternion.identity).GetComponent<EnemyBase>();
            enemyManager.RegisterEnemy(enemy);
        });


    }

}
