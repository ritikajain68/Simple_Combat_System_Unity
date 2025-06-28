using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerSpawner : MonoBehaviour
{
    public static bool IsGameOver = false;
    public int characterCount = 10;
    [Header("Spawner Settings")]
    public GameObject characterPrefab;
    public static PlayerSpawner Instance;

    public List<Transform> validSpawnPoints = new List<Transform>();
    public Transform spawnPointsParent;
    
    [Header("Game State")]
    public List<GameObject> characters_gameObject = new List<GameObject>();

    public List<GameObject> SpawnedPlayers { get; private set; } = new List<GameObject>();
    public List<PlayerSetup> players = new List<PlayerSetup>();

    [Header("Progress")]
    public Dictionary<string, PlayerSetup.PlayerStatsData> playerStatsDictionary = new Dictionary<string, PlayerSetup.PlayerStatsData>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AssignSpawnPoints();
    }

    public void AssignSpawnPoints()
    {
        if (spawnPointsParent == null)
        {
            Debug.LogError("SpawnPoints parent not assigned!");
            return;
        }

        validSpawnPoints.Clear();

        foreach (Transform child in spawnPointsParent)
        {
            if (child.CompareTag("SpawnPoint"))
            {
                validSpawnPoints.Add(child);
            }
        }

        if (validSpawnPoints.Count == 0)
        {
            Debug.LogError("No valid spawn points found!");
        }

        characterCount = validSpawnPoints.Count;
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < characterCount; i++)
        {
            GameObject character = Instantiate(characterPrefab, GetPosition(i), Quaternion.identity);
            character.transform.SetParent(validSpawnPoints[i].transform);
            validSpawnPoints[i].name = "SpawnPoint_" + i;
            character.name = "Player_" + i;
            characters_gameObject.Add(character);

            var setup = character.GetComponent<PlayerSetup>();
            var stats = new PlayerSetup.PlayerStatsData { playerName = character.name, killCount = 0 };

            setup.playerStatsData = stats;
            players.Add(setup);
            setup.FindNewTarget();
            playerStatsDictionary.Add(character.name, stats);
        }
    }

    private Vector3 GetPosition(int index)
    {
        if (validSpawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points defined!");
            return Vector3.zero;
        }

        if (index >= validSpawnPoints.Count)
        {
            Debug.LogWarning("Index out of range. Wrapping around.");
            return validSpawnPoints[index % validSpawnPoints.Count].position;
        }
        return validSpawnPoints[index].position;
    }
    public GameObject GetRandomTarget(PlayerSetup requester)
    {
        List<PlayerSetup> alive = players.FindAll(p => p != requester && p.isAlive);
        if (alive.Count == 0) return null;
        return alive[Random.Range(0, alive.Count)].gameObject;
    }
    public void CheckBattleState()
    {
        // Get all players that are still active and alive
        var alivePlayers = characters_gameObject
            .Where(p => p.activeInHierarchy && p.GetComponent<PlayerSetup>().isAlive)
            .ToList();

        // If only one player is alive → game over
        if (alivePlayers.Count == 1)
        {
            IsGameOver = true;
            string winnerName = alivePlayers[0].name;
            Debug.Log($"Winner is {winnerName}!");
            PlayerUIManager.Instance.ShowWinner(winnerName);
        }
        else if (alivePlayers.Count == 0)
        {
            Debug.Log("No players left alive!");
            PlayerUIManager.Instance.ShowWinner("No one");
        }
    }
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}