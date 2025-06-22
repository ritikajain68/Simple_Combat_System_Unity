using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public List<CharacterSetup> characters;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // You can randomize or manually assign weapons, names, etc.
        foreach (CharacterSetup c in characters)
        {
            c.weapon = Instantiate(c.weapon);
        }
    }

    public CharacterSetup GetNearestEnemy(CharacterSetup attacker)
    {
        return characters
            .Where(c => c != attacker && c.isAlive)
            .OrderBy(c => Vector3.Distance(attacker.transform.position, c.transform.position))
            .FirstOrDefault();
    }

    public void CheckBattleOutcome()
    {
        var alive = characters.Where(c => c.isAlive).ToList();
        if (alive.Count == 1)
        {
            Debug.Log("Winner: " + alive[0].characterName);
        }
        else if (alive.Count == 0)
        {
            Debug.Log("Draw! Everyone died!");
        }
    }
}
