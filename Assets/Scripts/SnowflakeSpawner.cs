using System.Collections.Generic;
using UnityEngine;

public class SnowflakeSpawner : MonoBehaviour
{
    public GameObject snowflakePrefab; // Snowflake prefab to spawn.
    public float spawnInterval = 1f; // Time interval between spawns.
    public float xSpawnRange = 8f; // Horizontal range for spawning.

    private float timer;

    private List<GameObject> spawnedSnowflakes = new List<GameObject>(); // Track spawned snowflakes.

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnSnowflake();
            timer = 0f;
        }
    }

    void SpawnSnowflake()
    {
        // Randomize X position within the range.
        float randomX = Random.Range(-xSpawnRange, xSpawnRange);

        // Instantiate the snowflake at the spawner's position with a random X offset.
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);
        GameObject snowflake = Instantiate(snowflakePrefab, spawnPosition, Quaternion.identity);

        spawnedSnowflakes.Add(snowflake);
    }

    public void DestroyAllSnowflakes()
    {
        foreach (GameObject snowflake in spawnedSnowflakes)
        {
            if (snowflake != null) // Check if the snowflake still exists.
            {
                Destroy(snowflake);
            }
        }

        // Clear the list.
        spawnedSnowflakes.Clear();

        Debug.Log("All snowflakes destroyed.");
    }
}
