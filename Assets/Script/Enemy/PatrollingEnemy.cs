using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    // For controlling enemy movement
    public Transform player;
    public float moveSpeed = 0.5f;
    private Rigidbody2D enemy_body;
    private Vector2 movement;
    public Collider2D guide;
    public MapGrid grid;

    // For animations
    private Animator anim;

    // Booleans for the flipping direction facing
    private bool facingLeft;

    // For Running Bresenham Algorithm
    // This will turn to true once the enemy spots Malakai
    private bool activated = false;
    // The higher the value, the smaller the chance of running the free sight
    public int freeSightChance = 100;
    // how far away the enemy can see
    public int sightRadius = 5;
    
    private bool nextStepIsOnTheGround = false;
    

    // Start is called before the first frame update
    void Start()
    {
        // Grab references for rigidbody and animator
        enemy_body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Freeze rotation and movement in y-axis
        enemy_body.freezeRotation = true;
        enemy_body.angularVelocity = 0f;
        enemy_body.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        activated |= Bresenham.determineActivation(freeSightChance, sightRadius, transform.position, player.position);
        if (activated) {
            // Rotate by calculated angle to face player
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            Flip(movement);  // Flip the image to match the direction to face

            // update animation
            anim.SetBool("walk", Mathf.Abs(direction[0]) > 0.1);
        }
    }

    private void FixedUpdate()
    {
        activated |=  Bresenham.determineActivation(freeSightChance, sightRadius, transform.position, player.position);
        if (activated && nextStepIsOnTheGround) {
            MoveEnemy(movement);  // Have enemy follow player
            Debug.Log("Activated and next step IS on the ground");
            anim.SetBool("walk", true);
        } else if (activated) {
            Debug.Log("Activated but next step is NOT on the ground");
            anim.SetBool("walk", false);
        } else {
            if (facingLeft){
                // move left
                MoveEnemy(Vector2.left);
            } else {
                // move right
                MoveEnemy(Vector2.right);
            }
            anim.SetBool("walk", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { // when a collider exits another collider, this function runs
        nextStepIsOnTheGround = false;
        Debug.Log("The collider exited");
        if (activated){
            Debug.Log(nextStepIsOnTheGround);
        } else {
            // in patrol mode
            if (facingLeft){
                Flip(Vector2.right); // face right
            } else {
                Flip(Vector2.left); // face left
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        nextStepIsOnTheGround = true;
        Debug.Log("The collider entered");
    }

    // Move enemy position based on where the player is
    private void MoveEnemy(Vector2 direction)
    {
        enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    // Flip the enemy character depending on which direction it is facing
    private void Flip(Vector2 direction)
    {
        if (facingLeft && direction[0] > 0f || !facingLeft && direction[0] <= 0f)
        {
            facingLeft = !facingLeft;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            // flipping means going the other way so this would always be true
            nextStepIsOnTheGround = true;
        }
    }
}
