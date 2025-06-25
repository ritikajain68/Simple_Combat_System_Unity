using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject characterPrefab;   // Root prefab that contains child with PlayerSetup
    public int characterCount = 10;
    public float spawnRadius = 10f;

    [Header("Center Point for Spawn Circle")]
    public Transform centerPoint;

    public List<GameObject> SpawnedPlayers { get; private set; } = new List<GameObject>();

    public void SpawnCharacters()
    {
        ClearExisting();

        float angleStep = 360f / characterCount;

        for (int i = 0; i < characterCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * spawnRadius,
                0f,
                Mathf.Sin(angle) * spawnRadius
            );

            Vector3 spawnPos = centerPoint.position + offset;

            GameObject newChar = Instantiate(characterPrefab, spawnPos, Quaternion.identity);
            newChar.name = $"Player_{i + 1}";

            SpawnedPlayers.Add(newChar);
        }
    }

    public void ClearExisting()
    {
        foreach (GameObject obj in SpawnedPlayers)
        {
            if (obj != null)
                Destroy(obj);
        }

        SpawnedPlayers.Clear();
    }
}
