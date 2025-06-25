using UnityEngine;
using System.Collections.Generic;

public class PlayerBattleManager : MonoBehaviour
{
    public static PlayerBattleManager Instance;
    public GameObject characterPrefab;
    public Transform spawnArea;
    public int characterCount = 10;

    private List<PlayerSetup> players = new List<PlayerSetup>();

    void Awake() => Instance = this;

    void Start()
    {
        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        float radius = 10f;
        for (int i = 0; i < characterCount; i++)
        {
            float angle = i * Mathf.PI * 2 / characterCount;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 pos = spawnArea.position + offset;

            GameObject go = Instantiate(characterPrefab, pos, Quaternion.identity);
            go.name = $"Fighter_{i + 1}";

            PlayerSetup setup = go.GetComponentInChildren<PlayerSetup>();
            if (setup != null)
            {
                players.Add(setup);
            }
        }
    }

    public Transform GetRandomTarget(PlayerSetup requester)
    {
        List<PlayerSetup> alive = players.FindAll(p => p != requester && p.isAlive);
        if (alive.Count == 0) return null;
        return alive[Random.Range(0, alive.Count)].transform;
    }

    public void CheckBattleState()
    {
        var alive = players.FindAll(p => p.isAlive);
        if (alive.Count == 1)
        {
            Debug.Log("🏆 Winner: " + alive[0].name);
        }
    }
}
