using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrollingEnemy : PlatformPatrollingEnemy
{
    bool hitAWall;
    float memory3 = 5f; float startTime3;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        hitAWall = false;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // bool usedToBeActivated = activated;
        // bool bresenhamResult = Bresenham.determineActivation(0, sightRadius, transform.position, player.position);
        // activated |= bresenhamResult;

        // if (activated && bresenhamResult){
        //     // Enemy can still see Malakai. Reset the timer.
        //     Debug.Log("I can see Malakai");
        //     startTime1 = Time.time;
        // } else if (activated && !bresenhamResult && (Time.time - startTime1 > memory1) ) {
        //     // enemy used to be activated
        //     // enemy no longer sees Malakai
        //     // if its been memory1-seconds since enemy last saw malakai
        //     Debug.Log("I forgot I saw Malakai.");
        //     // the enemy will forget Malakai
        //     anim.SetBool("Idle", true); // figure out how to do this properly
        //     activated = false;
        // }

        // if (activated) {
        //     // Rotate by calculated angle to face player
        //     Vector3 direction = player.position - transform.position;
        //     direction.Normalize();
        //     movement = direction;
        //     Flip(movement);  // Flip the image to match the direction to face
        //     // update animation
        //     anim.SetBool("walk", Mathf.Abs(direction[0]) > 0.1);
        // }
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
            startTime3 = Time.time;
        } else if (activated && !bresenhamResult && (Time.time - startTime3 > memory3) ) {
            // enemy used to be activated 
            // enemy no longer sees Malakai
            // if its been memory2-seconds since enemy last saw malakai
            Debug.Log("I cannot see Malakai anymore");
            Debug.Log(Time.time - startTime3);
            // the enemy will forget Malakai
            activated = false;
        }

        if (activated && !hitAWall) {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            Flip(movement);
            MoveEnemy(movement);  // Have enemy follow player
            anim.SetBool("walk", true);
        } else if (activated) {
            // at a wall
            Debug.Log("I am activated but I hit a wall");
            anim.SetBool("walk", false);
        } else {
            if (oldActivation && !activated){
                // enemy JUST forgot about Malakai
                // enemy should go the other way
                if (facingLeft){
                    // move them the other way
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // when a collider exits another collider, this function runs
        if (
        // collision.gameObject.layer == LayerMask.NameToLayer("Enemy")||
        // collision.gameObject.layer == LayerMask.NameToLayer("Hazards") ||
        collision.GetComponent<DoorController>()
        // || collision.GetComponent<StationaryHazard>()
        )
        {
            Debug.Log("Ignoring", collision.gameObject);
            // ignore, it's just a door/stationary object
            return;
        }
        hitAWall = true;
        if (activated){
            Debug.Log("I am activated and have hit a wall. I cannot continue.");
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
        if (collision.GetComponent<DoorController>()
        // || collision.GetComponent<StationaryHazard>()
        )
        {
            // ignore, it's just a door/stationary object
            return;
        }
        hitAWall = false;
        Debug.Log("The collider entered");
    }

    // Flip the enemy character depending on which direction it is facing
    protected override void Flip(Vector2 direction)
    {
        base.Flip(direction);
        if (facingLeft && direction[0] > 0f || !facingLeft && direction[0] <= 0f)
        {
            hitAWall = true;
        }
    }
}
