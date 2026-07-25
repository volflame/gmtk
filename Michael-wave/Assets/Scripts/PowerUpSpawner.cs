using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [Header("Powerup Setup")]
    public GameObject[] powerupPrefabs; // drag your 4 prefabs here in the Inspector

    [Header("Spawn Settings")]
    public float spawnInterval = 10f;
    public float minSpawnDistance = 3f;
    public float maxSpawnDistance = 8f;

    private Transform player;
    private float spawnTimer = 0f;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        spawnTimer = spawnInterval; // spawn one immediately on start; set to 0 if you want to wait the full interval first
    }

    void Update()
    {
        if (!player) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnRandomPowerup();
        }
    }

    void SpawnRandomPowerup()
    {
        if (powerupPrefabs.Length == 0) return;

        GameObject prefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector3 spawnPos = player.position + (Vector3)(randomDirection * randomDistance);

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}