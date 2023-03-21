using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // For controlling player attack
    public int attackDamage = 25;
    public float attackRange = 0.1f;
    public float attackCooldown = 0.5f;
    private Transform attackPoint;
    private float attackTimer = 0f;

    // Additional Unity Components
    private Animator anim;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        // Grab references for rigidbody and animator from object
        anim = GetComponent<Animator>();
        attackPoint = transform.Find("AttackPoint");
    }

    // Update is called once per frame
    void Update()
    {
        // Attack on mouse click or space bar
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && attackTimer > attackCooldown)
        {
            Attack();
            attackTimer = 0;
        }

        attackTimer += Time.deltaTime;
    }

    void Attack()
    {
        // Play attack animation
        anim.SetTrigger("attack");

        // Detect enemies in range of the attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }
}