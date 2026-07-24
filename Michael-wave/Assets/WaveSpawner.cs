using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    [Header("Enemy Setup")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public int enemiesPerWave = 5;
    public int enemiesIncreasePerWave = 2; // optional scaling
    public float timeBetweenWaves = 3f;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool waveInProgress = false;

    void Awake()
    {
        // Simple singleton so Enemy.cs can find this easily
        Instance = this;
    }

    void Start()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        waveInProgress = true;

        int enemyCount = enemiesPerWave + (currentWave - 1) * enemiesIncreasePerWave;
        enemiesAlive = enemyCount;

        Debug.Log("Starting Wave " + currentWave + " with " + enemyCount + " enemies");

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Add a small random offset so enemies don't spawn on the exact same point
        Vector2 randomOffset = Random.insideUnitCircle * 1.5f; // tweak radius to taste
        Vector3 spawnPos = spawnPoint.position + (Vector3)randomOffset;

        Instantiate(enemyPrefab, spawnPos, spawnPoint.rotation);
    }

    // Called by Enemy.cs when an enemy dies
    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && waveInProgress)
        {
            waveInProgress = false;
            Debug.Log("Wave " + currentWave + " cleared!");
            Invoke(nameof(StartNextWave), timeBetweenWaves);
        }
    }
}