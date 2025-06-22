using UnityEngine;

public class CharacterSetup : MonoBehaviour
{
    public string characterName;
    public float health = 100f;
    public float speed = 3f;
    public Weapon weapon;
    public bool isAlive = true;

    private void Update()
    {
        if (!isAlive) return;

        weapon.TryAttack(this);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isAlive = false;
            gameObject.SetActive(false);
            BattleManager.Instance.CheckBattleOutcome();
        }
    }
}
