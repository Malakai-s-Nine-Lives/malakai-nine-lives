using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrollingEnemy : PlatformPatrollingEnemy
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")||collision.GetComponent<DoorController>()|| collision.GetComponent<StationaryHazard>())
        {
            Debug.Log("Ignoring", collision.gameObject);
            // ignore, it's just a door/stationary object
            return;
        }
        // when a collider exits another collider, this function runs
        nextStepIsOnTheGround = true;
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

    protected override void OnTriggerExit2D(Collider2D collision){
        if (collision.GetComponent<DoorController>()|| collision.GetComponent<StationaryHazard>())
        {
            // ignore, it's just a door/stationary object
            return;
        }
        nextStepIsOnTheGround = true;
        Debug.Log("The collider entered");
    }
}
