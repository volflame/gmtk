using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    [System.Serializable]
    public class EnemySpawnEntry
    {
        public GameObject enemyPrefab;
        [Tooltip("Relative chance of this enemy being picked. Higher = more common.")]
        public float weight = 1f;
    }

    [Header("Enemy Setup")]
    public EnemySpawnEntry[] enemyTypes;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public int enemiesPerWave = 5;
    public int enemiesIncreasePerWave = 2;
    public float timeBetweenWaves = 3f;

    [Header("Boss Settings")]
    public int totalWaves = 5;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public CinemachineImpulseSource bossSpawnImpulseSource;
    public float bossSpawnImpulseForce = 3f; // strong — tune to taste

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool waveInProgress = false;
    private bool bossSpawned = false;

    void Awake()
    {
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

        if (currentWave >= totalWaves)
        {
            SpawnBoss();
            return;
        }

        int enemyCount = enemiesPerWave + (currentWave - 1) * enemiesIncreasePerWave;
        enemiesAlive = enemyCount;

        Debug.Log("Starting Wave " + currentWave + " with " + enemyCount + " enemies");

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogWarning("No boss prefab assigned!");
            return;
        }

        bossSpawned = true;
        enemiesAlive = 1;

        Transform spawnPoint = bossSpawnPoint != null
            ? bossSpawnPoint
            : spawnPoints[Random.Range(0, spawnPoints.Length)];

        Debug.Log("Final wave — spawning boss");
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        boss.SetActive(true); // usually unnecessary unless your prefab is saved in a disabled state

        if (bossSpawnImpulseSource != null)
        {
            bossSpawnImpulseSource.GenerateImpulseWithForce(bossSpawnImpulseForce);
        }
    }

        [Header("Ground Validation")]
    public Tilemap groundTilemap; // drag your ground Tilemap here

    private bool IsValidGroundPosition(Vector3 worldPos)
    {
        if (groundTilemap == null) return true; // no tilemap assigned, skip validation

        Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);
        TileBase tile = groundTilemap.GetTile(cellPos);
        return tile != null; // true only if there's an actual ground tile at this cell
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyTypes.Length == 0) return;

        GameObject enemyPrefab = PickRandomEnemyType();
        if (enemyPrefab == null) return;

        Vector3 spawnPos = GetValidSpawnPosition();
        if (spawnPos == Vector3.positiveInfinity) return; // couldn't find a valid spot, skip this spawn

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    private Vector3 GetValidSpawnPosition()
    {
        const int maxAttempts = 10;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector2 randomOffset = Random.insideUnitCircle * 1.5f;
            Vector3 candidate = spawnPoint.position + (Vector3)randomOffset;

            if (IsValidGroundPosition(candidate))
            {
                return candidate;
            }
        }

        // fallback: just use the spawn point itself with no offset if nothing valid found after several tries
        Transform fallback = spawnPoints[Random.Range(0, spawnPoints.Length)];
        return IsValidGroundPosition(fallback.position) ? fallback.position : Vector3.positiveInfinity;
    }

    private GameObject PickRandomEnemyType()
    {
        float totalWeight = 0f;
        foreach (var entry in enemyTypes)
        {
            totalWeight += entry.weight;
        }

        if (totalWeight <= 0f) return null;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in enemyTypes)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
            {
                return entry.enemyPrefab;
            }
        }

        return enemyTypes[enemyTypes.Length - 1].enemyPrefab;
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && waveInProgress)
        {
            waveInProgress = false;

            if (bossSpawned)
            {
                Debug.Log("Boss defeated! Game complete.");
                return;
            }

            Debug.Log("Wave " + currentWave + " cleared!");
            Invoke(nameof(StartNextWave), timeBetweenWaves);
        }
    }
}