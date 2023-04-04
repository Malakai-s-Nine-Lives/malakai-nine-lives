using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyMovement : MonoBehaviour
{
    // For controlling enemy movement
    public Transform player;
    public float moveSpeed = 0.5f;
    private Rigidbody2D enemy_body;
    protected Vector2 movement;

    // For animations
    protected Animator anim;

    // Booleans for the flipping direction facing
    protected bool facingLeft;

    // For Running Bresenham Algorithm
    // This will turn to true once the enemy spots Malakai
    protected bool activated;
    // The higher the value, the smaller the chance of running the free sight
    public int freeSightChance = 0;
    // how far away the enemy can see
    public int sightRadius = 100;
    // for the enemy to forget Malakai
    protected float memory = 5f;
    protected float startTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Grab references for rigidbody and animator
        enemy_body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Freeze rotation and movement in y-axis
        enemy_body.freezeRotation = true;
        enemy_body.angularVelocity = 0f;
        enemy_body.constraints = RigidbodyConstraints2D.FreezePositionY;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Hazards"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);
        activated = false; // start enemy in idle mode
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }


    protected virtual void FixedUpdate()
    {
        bool usedToBeActivated = activated;
        bool bresenhamResult = Bresenham.determineActivation(0, sightRadius, transform.position, player.position);
        activated |= bresenhamResult;

        if (activated && bresenhamResult){
            // Enemy can still see Malakai. Reset the timer.
            Debug.Log("I can see Malakai");
            startTime = Time.time;
        } else if (activated && !bresenhamResult && (Time.time - startTime > memory) ) {
            // enemy used to be activated
            // enemy no longer sees Malakai
            // if its been memory-seconds since enemy last saw malakai
            Debug.Log("I forgot I saw Malakai.");
            // the enemy will forget Malakai
            anim.SetBool("Idle", true); // figure out how to do this properly
            activated = false;
        }

        if (activated) {
            // Rotate by calculated angle to face player
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            Flip(movement);  // Flip the image to match the direction to face
            // update animation
            anim.SetBool("walk", Mathf.Abs(direction[0]) > 0.1);
            MoveEnemy(movement);
        }
    }

    // Move enemy position based on where the player is
    protected void MoveEnemy(Vector2 direction)
    {
        enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    // Flip the enemy character depending on which direction it is facing
    protected virtual void Flip(Vector2 direction)
    {
        if (facingLeft && direction[0] > 0f || !facingLeft && direction[0] <= 0f)
        {
            facingLeft = !facingLeft;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
