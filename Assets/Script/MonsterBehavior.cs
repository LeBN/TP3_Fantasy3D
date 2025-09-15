using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonsterAI : MonoBehaviour
{
    [Header("Cibles & Stats")]
    public Transform player;
    public Health playerHealth;
    public Health myHealth;
    public int attackDamage = 10;
    public float attackCooldown = 1.0f;
    public float attackRange = 1.8f;
    public float moveSpeed = 3.5f;

    [Header("État")]
    public bool aggro;
    float lastAttackTime;

    void Awake()
    {
        if (!myHealth) myHealth = GetComponent<Health>();
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        if (!playerHealth && player) playerHealth = player.GetComponent<Health>();
    }

    void Update()
    {
        if (!aggro || !myHealth || !myHealth.IsAlive || !player || (playerHealth && !playerHealth.IsAlive))
            return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float dist = toPlayer.magnitude;

        if (dist > attackRange)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toPlayer), 10f * Time.deltaTime);
            transform.position += toPlayer.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;
        if (playerHealth) playerHealth.TakeDamage(attackDamage);
        // TODO: déclencher anim d'attaque ici
    }

    public void SetAggro(bool value) => aggro = value;
}
