using UnityEngine;

public class StationaryHazard : MonoBehaviour
{
    public int damage = 10;
    public LayerMask playerLayer;
    private float damageTimer = 0;
    public float damageCooldown = 2;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Malakai")
        {
            damageTimer = 0;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        damageTimer += Time.deltaTime;
        if (collision.gameObject.name == "Malakai" && damageTimer > damageCooldown)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            damageTimer = 0;
        }
    }
}
