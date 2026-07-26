using System.Collections;
using UnityEngine;
using Cinemachine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 6f;
    public int columnCount = 20;
    public int lineCount = 20;
    public float spacing = 1.2f;
    public float delayBetweenSpawns = 0.1f;
    public CinemachineImpulseSource impulseSource;
    public float impulseForce = 0.5f;

    private Bounds CameraBounds()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        return new Bounds(cam.transform.position, new Vector3(width, height, 0));
    }

    // Bullets appear top-to-bottom along the left edge, staggered in time, all moving right — the "ramp" look
    public IEnumerator CascadeRampAttack()
    {
        Bounds b = CameraBounds();
        float startX = b.min.x - 1f;
        float topY = b.max.y - 0.5f;

        for (int i = 0; i < columnCount; i++)
        {
            Vector3 spawnPos = new Vector3(startX, topY - (i * spacing), 0);
            SpawnProjectile(spawnPos, Vector2.right);
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    // Bullets spawn across the top, staggered left-to-right, all falling straight down
    public IEnumerator CascadeDownwardAttack()
    {
        Bounds b = CameraBounds();
        float topY = b.max.y + 1f;
        float startX = b.min.x + 0.5f;

        for (int i = 0; i < columnCount; i++)
        {
            Vector3 spawnPos = new Vector3(startX + (i * spacing), topY, 0);
            SpawnProjectile(spawnPos, Vector2.down);
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    private void SpawnProjectile(Vector3 pos, Vector2 direction)
    {
        GameObject proj = Instantiate(projectilePrefab, pos, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }

    public IEnumerator VerticalLineOnPlayer(Vector3 playerPos)
{
    if (impulseSource != null)
    {
        impulseSource.GenerateImpulseWithForce(impulseForce);
    }
    float totalHeight = (lineCount - 1) * spacing;
    float startY = playerPos.y + (totalHeight / 2f); // start from top of the line, centered on player
    float spawnX = playerPos.x; // could offset to one side if you don't want it to spawn directly on top of the player

    for (int i = 0; i < lineCount; i++)
    {
        Vector3 spawnPos = new Vector3(spawnX, startY - (i * spacing), 0);
        SpawnProjectile(spawnPos, Vector2.left); // pick whichever direction makes sense — see note below
        yield return new WaitForSeconds(delayBetweenSpawns);
    }
}
}