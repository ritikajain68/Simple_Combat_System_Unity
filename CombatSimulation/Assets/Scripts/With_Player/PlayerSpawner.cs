using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject characterPrefab;
    public int characterCount = 10;
    public float spawnRadius = 10f;

    [Header("Spawn Area Reference")]
    public Transform centerPoint;

    public List<GameObject> spawnedPlayers { get; private set; } = new List<GameObject>();

    public void SpawnCharacters()
    {
        ClearExisting();

        float radius = spawnRadius;
        float angleStep = 360f / characterCount;

        for (int i = 0; i < characterCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(
                centerPoint.position.x + Mathf.Cos(angle) * radius,
                centerPoint.position.y,
                centerPoint.position.z + Mathf.Sin(angle) * radius
            );

            GameObject newChar = Instantiate(characterPrefab, pos, Quaternion.identity);
            newChar.name = $"Player{i + 1}";
            spawnedPlayers.Add(newChar); // ✅ lowercase fix
        }
    }

    private Vector3 GetRandomPosition(Vector3 center, float radius)
    {
        Vector2 randCircle = Random.insideUnitCircle * radius;
        return new Vector3(center.x + randCircle.x, center.y, center.z + randCircle.y);
    }

    public void ClearExisting()
    {
        foreach (GameObject obj in spawnedPlayers)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedPlayers.Clear();
    }
}
