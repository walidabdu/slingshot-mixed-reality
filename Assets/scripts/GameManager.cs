using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab;                  // Prefab to spawn
    [Range(0f, 1f)] public float probability;  // Probability weight
}

public class GameManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public SpawnablePrefab[] spawnables;       // Array of prefabs with probabilities
    // Assign a GameObject in the Inspector whose Transform.position will be used as the spawn location.
    // If null, the fallback `spawnPositionFallback` will be used.
    public GameObject spawnPoint;
    public Vector3 spawnPositionFallback = Vector3.zero; // Fallback position if spawnPoint is not assigned

    [Header("Spawn Interval (Random Range)")]
    public float minSpawnInterval = 3f;        // Minimum time between spawns
    public float maxSpawnInterval = 6f;        // Maximum time between spawns

    void Start()
    {
        StartCoroutine(SpawnPrefabCoroutine());
    }

    IEnumerator SpawnPrefabCoroutine()
    {
        while (true)
        {
            GameObject prefabToSpawn = GetRandomPrefab();

            if (prefabToSpawn != null)
            {
                Vector3 pos = (spawnPoint != null) ? spawnPoint.transform.position : spawnPositionFallback;
                Instantiate(prefabToSpawn, pos, prefabToSpawn.transform.rotation);
            }

            // Wait for a random interval between min and max
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Selects a prefab based on probability
    GameObject GetRandomPrefab()
    {
        if (spawnables.Length == 0) return null;

        float total = 0f;
        foreach (var s in spawnables)
        {
            total += s.probability;
        }

        if (total <= 0f)
        {
            Debug.LogWarning("Total probability is zero. Nothing will spawn.");
            return null;
        }

        float randomPoint = Random.value * total;

        foreach (var s in spawnables)
        {
            if (randomPoint < s.probability)
            {
                return s.prefab;
            }
            else
            {
                randomPoint -= s.probability;
            }
        }

        return null;
    }
}
