using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerupSpawner : MonoBehaviour
{
    [Header("Powerup Setup")]
    public GameObject[] powerupPrefabs; // drag your 4 prefabs here in the Inspector

    [Header("Spawn Settings")]
    public float spawnInterval = 10f;
    public float minSpawnDistance = 3f;
    public float maxSpawnDistance = 8f;

    [Header("Ground Validation")]
    public Tilemap groundTilemap; // drag your ground Tilemap here

    private Transform player;
    private float spawnTimer = 0f;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        spawnTimer = spawnInterval;
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

        Vector3 spawnPos = GetValidSpawnPosition();
        if (spawnPos == Vector3.positiveInfinity) return; // couldn't find a valid ground spot, skip this spawn

        GameObject prefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    private Vector3 GetValidSpawnPosition()
    {
        const int maxAttempts = 10;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 candidate = player.position + (Vector3)(randomDirection * randomDistance);

            if (IsValidGroundPosition(candidate))
            {
                return candidate;
            }
        }

        return Vector3.positiveInfinity; // no valid spot found after several tries — caller should skip spawning
    }

    private bool IsValidGroundPosition(Vector3 worldPos)
    {
        if (groundTilemap == null) return true; // no tilemap assigned, skip validation

        Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);
        TileBase tile = groundTilemap.GetTile(cellPos);
        return tile != null;
    }
}