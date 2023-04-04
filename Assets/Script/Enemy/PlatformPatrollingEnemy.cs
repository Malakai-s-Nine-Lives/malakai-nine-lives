using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPatrollingEnemy : WalkingEnemyMovement
{
    
    protected bool nextStepIsOnTheGround = false;
    float memory2 = 5f; float startTime2;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        nextStepIsOnTheGround = false;
        base.Start();
    }

    protected override void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;
    }
    
    protected override void FixedUpdate()
    {

        bool oldActivation = activated;
        // Bresenham has to be ran deterministically to implement forgetfulness properly
        bool bresenhamResult = Bresenham.determineActivation(0, sightRadius, transform.position, player.position);
        activated |= bresenhamResult;
        
        if (activated && bresenhamResult){
            // Enemy can still see Malakai. Reset the timer.
            Debug.Log("I can see Malakai");
            startTime2 = Time.time;
        } else if (activated && !bresenhamResult && (Time.time - startTime2 > memory2) ) {
            // enemy used to be activated 
            // enemy no longer sees Malakai
            // if its been memory2-seconds since enemy last saw malakai
            Debug.Log("I cannot see Malakai anymore");
            Debug.Log(Time.time - startTime2);
            // the enemy will forget Malakai
            activated = false;
        }
        
        if (activated && nextStepIsOnTheGround) {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            Flip(movement);
            MoveEnemy(movement);  // Have enemy follow player
            Debug.Log("Activated and next step IS on the ground");
            anim.SetBool("walk", true);
        } else if (activated) {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            Flip(movement);

            Debug.Log("Activated but next step is NOT on the ground");
            anim.SetBool("walk", false);
        } else {
            // patrol mode
            if (oldActivation && !activated){
                // enemy JUST forgot about Malakai
                // enemy should go the other way
                if (facingLeft){
                    Flip(Vector2.right);
                    MoveEnemy(Vector2.right);
                } else {
                    Flip(Vector2.left);
                    MoveEnemy(Vector2.left);
                }
            } else {
                if (facingLeft){
                    // move left
                    MoveEnemy(Vector2.left);
                } else {
                    // move right
                    MoveEnemy(Vector2.right);
                }
            }
            anim.SetBool("walk", true);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    { 
        if (collision.GetComponent<DoorController>()|| collision.GetComponent<StationaryHazard>())
        {
            // ignore, it's just a door/stationary object
            return;
        }
        // when a collider exits another collider, this function runs
        nextStepIsOnTheGround = false;
        Debug.Log("The collider exited");
        if (activated){
            // in hunt mode
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

    protected virtual void OnTriggerEnter2D(Collider2D collision){
        if (collision.GetComponent<DoorController>()|| collision.GetComponent<StationaryHazard>())
        {
            // ignore, it's just a door/stationary object
            return;
        }
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
