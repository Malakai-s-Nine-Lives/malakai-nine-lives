using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // For controlling enemy attacks
    private float attackTimer = 0f;
    public float attackCooldown = 1f;
    public int attackDamage = 25;
    public float attackRange = 0.1f;
    public LayerMask playerLayer;

    // Additional Unity Components
    private Animator anim;
    private Transform attackPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Grab references for animator and attack point area from object
        anim = GetComponent<Animator>();
        attackPoint = transform.Find("AttackPoint");
    }

    // Update is called once per frame
    void Update()
    {
        // attack if cooldown is up
        if (attackTimer > attackCooldown)
        {
            attackTimer = 0;
            Attack();
        }

        // Increment attack time each update
        attackTimer += Time.deltaTime;
    }

    void Attack()
    {
        // Detect player in the range of the attack
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (hitPlayer)
        {
            // Play attack animation
            anim.SetTrigger("attack");

            // Damage player
            hitPlayer.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }
}
