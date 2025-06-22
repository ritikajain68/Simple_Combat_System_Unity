using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject characterPrefab;
    public BattleManager battleManager;
    public float radius = 10f;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2 / 10;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            GameObject characterObj = Instantiate(characterPrefab, position, Quaternion.identity);
            CharacterSetup character = characterObj.GetComponent<CharacterSetup>();
            character.weapon = characterObj.GetComponentInChildren<Weapon>();
            character.characterName = "Fighter " + (i + 1);
            battleManager.characters.Add(character);
        }
    }
}
