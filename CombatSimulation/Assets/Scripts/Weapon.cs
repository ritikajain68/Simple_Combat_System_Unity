using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float cooldown = 1.5f;
    private float lastAttackTime = 0f;

    public void TryAttack(CharacterSetup self)
    {
        if (Time.time - lastAttackTime < cooldown) return;

        var target = BattleManager.Instance.GetNearestEnemy(self);
        if (target != null)
        {
            target.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}
