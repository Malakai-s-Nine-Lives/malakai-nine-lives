using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritedPatrollingScript : WalkingEnemyMovement
{
    
    private bool nextStepIsOnTheGround = false;

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    
    protected override void FixedUpdate()
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
    // private void MoveEnemy(Vector2 direction)
    // {
    //     enemy_body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    // }

    // Flip the enemy character depending on which direction it is facing
    protected override void Flip(Vector2 direction)
    {
        base.Flip(direction);
        if (facingLeft && direction[0] > 0f || !facingLeft && direction[0] <= 0f)
        {
            nextStepIsOnTheGround = true;
        }
    }
}
