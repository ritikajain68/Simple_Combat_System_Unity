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
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 spawnPos = spawnArea.position + pos;

            GameObject go = Instantiate(characterPrefab, spawnPos, Quaternion.identity);
            go.name = $"Fighter_{i + 1}";
            players.Add(go.GetComponent<PlayerSetup>());
        }
    }


    public Transform GetRandomTarget(PlayerSetup requester)
    {
        List<PlayerSetup> alive = players.FindAll(c => c != requester && c.isAlive);
        if (alive.Count == 0) return null;
        return alive[Random.Range(0, alive.Count)].transform;
    }

    public void CheckBattleState()
    {
        List<PlayerSetup> alive = players.FindAll(c => c.isAlive);
        if (alive.Count == 1)
        {
            Debug.Log("🏆 Winner: " + alive[0].name);
            PlayerUIManager.Instance.ShowWinner(alive[0].name);
        }
    }
}
