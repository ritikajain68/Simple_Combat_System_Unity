using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public GameObject characterPrefab;
    public Transform spawnArea;
    public int characterCount = 10;

    private List<CharacterSetup> characters = new List<CharacterSetup>();

    void Awake() => Instance = this;

    void Start()
    {
        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        for (int i = 0; i < characterCount; i++)
        {
            Vector3 pos = spawnArea.position + Random.insideUnitSphere * 10f;
            pos.y = 0f;
            GameObject go = Instantiate(characterPrefab, pos, Quaternion.identity);
            go.name = $"Fighter_{i + 1}";
            characters.Add(go.GetComponent<CharacterSetup>());
        }
    }

    public Transform GetRandomTarget(CharacterSetup requester)
    {
        List<CharacterSetup> alive = characters.FindAll(c => c != requester && c.isAlive);
        if (alive.Count == 0) return null;
        return alive[Random.Range(0, alive.Count)].transform;
    }

    public void CheckBattleState()
    {
        List<CharacterSetup> alive = characters.FindAll(c => c.isAlive);
        if (alive.Count == 1)
        {
            Debug.Log("🏆 Winner: " + alive[0].name);
            UIManager.Instance.ShowWinner(alive[0].name);
        }
    }
}
