using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f;
    public float speed = 10f;
    private Transform target;

    public void Init(Transform target, CharacterSetup shooter)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            target.GetComponent<CharacterSetup>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
