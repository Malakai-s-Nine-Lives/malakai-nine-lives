using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : WalkingEnemyMovement
{
    protected bool allowedToTakeNextStep;

    void Start()
    {
        base.Start();
        allowedToTakeNextStep = true;
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;
    }

    protected virtual void FixedUpdate()
    {
        bool oldActivation = activated;
        // Bresenham has to be ran deterministically to implement forgetfulness properly
        bool bresenhamResult = Bresenham.determineActivation(0, sightRadius, transform.position, player.position);
        activated |= bresenhamResult;
        
        if (activated && bresenhamResult){
            // Enemy can still see Malakai. Reset the timer.
            Debug.Log("I can see Malakai");
            startTime = Time.time;
        } else if (activated && !bresenhamResult && (Time.time - startTime > memory) ) {
            // enemy used to be activated 
            // enemy no longer sees Malakai
            // if its been memory2-seconds since enemy last saw malakai
            Debug.Log("I forgot I saw Malakai");
            // go back to patrol mode
            activated = false;
        }

        if (activated) {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            Flip(movement);
            if (allowedToTakeNextStep) {
                MoveEnemy(movement);  // Have enemy follow player
                Debug.Log("Activated and next step IS allowed");
                anim.SetBool("walk", true);
            } else {
                Debug.Log("Activated but at the boundary");
                anim.SetBool("walk", false);
            }

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

    protected void HitABoundary(Collider2D collision)
    {
        if (collision.GetComponent<DoorController>()) return;
        allowedToTakeNextStep = false;
        if (activated){
            Debug.Log("I am activated and at the boundary. I cannot continue.");
        } else {
            // in patrol mode
            if (facingLeft){
                Flip(Vector2.right); // face right
            } else {
                Flip(Vector2.left); // face left
            }
        }
    }

    protected void ExitTheBoundary(Collider2D collision)
    {
        if (collision.GetComponent<DoorController>()) return;
        allowedToTakeNextStep = true;
        Debug.Log("The collider entered");
    }

    // Flip the enemy character depending on which direction it is facing
    protected override void Flip(Vector2 direction)
    {
        base.Flip(direction);
        if (facingLeft && direction[0] > 0f || !facingLeft && direction[0] <= 0f)
        {
            allowedToTakeNextStep = true;
        }
    }
}
