using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject characterPrefab;
    public int characterCount = 10;
    public float spawnRadius = 10f;

    [Header("Spawn Area Reference")]
    public Transform centerPoint;

    public List<GameObject> SpawnedCharacters { get; private set; } = new List<GameObject>();

    public void SpawnCharacters()
    {
        ClearExisting();

        for (int i = 0; i < characterCount; i++)
        {
            Vector3 randomPos = GetRandomPosition(centerPoint.position, spawnRadius);
            GameObject newChar = Instantiate(characterPrefab, randomPos, Quaternion.identity);
            newChar.name = $"Fighter_{i + 1}";
            SpawnedCharacters.Add(newChar);
        }
    }

    private Vector3 GetRandomPosition(Vector3 center, float radius)
    {
        Vector2 randCircle = Random.insideUnitCircle * radius;
        return new Vector3(center.x + randCircle.x, center.y, center.z + randCircle.y);
    }

    public void ClearExisting()
    {
        foreach (GameObject obj in SpawnedCharacters)
        {
            if (obj != null)
                Destroy(obj);
        }
        SpawnedCharacters.Clear();
    }
}
