using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    [Header("Game Settings")]
    public GameObject spawnParent;
    public GameObject characterPrefab;
    public List<Transform> validSpawnPoints;
    public Transform spawnPointsParent;
    public int numberOfCharactersToSpawn;

    [Header("Game State")]
    public List<GameObject> characters_gameObject = new List<GameObject>();
    public List<CharacterSetup> characters = new List<CharacterSetup>();

    [Header("Progress")]
    public Dictionary<string, CharacterSetup.CharacterStatsData> characterStatsDictionary = new Dictionary<string, CharacterSetup.CharacterStatsData>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AssignSpawnPoints();
        //SpawnPlayers();
    }

    public void AssignSpawnPoints()
    {
        if (spawnPointsParent == null)
        {
            Debug.LogError("SpawnPoints parent GameObject not found!");
            return;
        }
        validSpawnPoints = new List<Transform>();

        foreach (Transform child in spawnPointsParent.transform)
        {
            if (child.CompareTag("SpawnPoint"))
            {
                validSpawnPoints.Add(child);
            }
        }
        if (validSpawnPoints.Count <= 0)
        {
            Debug.LogError("No spawn points found in the spawnParent!");
            return;
        }
        numberOfCharactersToSpawn = validSpawnPoints.Count;
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < numberOfCharactersToSpawn; i++)
        {
            GameObject character = Instantiate(characterPrefab, GetPosition(i), Quaternion.identity);
            character.transform.SetParent(validSpawnPoints[i].transform);
            validSpawnPoints[i].name = "SpawnPoint_" + i;
            character.name = "Player_" + i;
            characters_gameObject.Add(character);

            var setup = character.GetComponent<CharacterSetup>();
            var stats = new CharacterSetup.CharacterStatsData { characterName = character.name, KillCount = 0 };

            setup.characterStatsData = stats;
            characters.Add(setup);
            setup.FindNewTarget();
            characterStatsDictionary.Add(character.name, stats);
        }
    }

    private Vector3 GetPosition(int characterIndex)
    {
        if (validSpawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available!");
            return Vector3.zero;
        }

        if (characterIndex >= validSpawnPoints.Count)
        {
            Debug.LogWarning("Character index exceeds spawn points count. Wrapping around.");
            return validSpawnPoints[characterIndex % validSpawnPoints.Count].position;
        }
        return validSpawnPoints[characterIndex].position;
    }
    public GameObject GetRandomTarget(CharacterSetup requester)
    {
        List<CharacterSetup> alive = characters.FindAll(c => c != requester && c.isAlive);
        if (alive.Count == 0) return null;
        return alive[Random.Range(0, alive.Count)].gameObject;
    }

    public void CheckBattleState()
    {
        List<CharacterSetup> alive = characters.FindAll(c => c.isAlive);
        if (alive.Count == 1)
        {
            string winner = alive[0].name;
            Debug.Log("Final Survivor: " + winner);
            UIManager.Instance.ShowWinner(winner);
        }
    }
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}