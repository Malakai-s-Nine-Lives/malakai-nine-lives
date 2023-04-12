using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This module satisfies:
 *   - Functional requirement 2.4
 */
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

    public bool isActivated()
    {
        return activated;
    }

    protected virtual void FixedUpdate()
    {
        bool oldActivation = activated;
        // Bresenham has to be ran deterministically to implement forgetfulness properly
        bool bresenhamResult = Bresenham.determineActivation(0, sightRadius, transform.position, player.position);
        activated |= bresenhamResult;
        
        if (activated && bresenhamResult){
            // Enemy can still see Malakai. Reset the timer.
            startTime = Time.time;
        } else if (activated && !bresenhamResult && (Time.time - startTime > memory) ) {
            // enemy used to be activated 
            // enemy no longer sees Malakai
            // if its been memory2-seconds since enemy last saw malakai
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
                anim.SetBool("walk", true);
            } else {
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
        if (!activated) {
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
