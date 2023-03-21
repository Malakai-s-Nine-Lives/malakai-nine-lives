using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyMovement : MonoBehaviour
{
    // For controlling enemy movement
    public Transform player;
    public float moveSpeed = 0.5f;
    private Rigidbody2D enemy_body;
    private Vector2 movement;

    // For animations
    private Animator anim;

    // Booleans for the flipping direction facing
    private bool facingLeft;

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
        // Rotate by caluclated angle to face player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;
        Flip(movement);  // Flip the image to match the direction to face

        // update animation
        anim.SetBool("walk", Mathf.Abs(direction[0]) > 0.1);
    } 

    private void FixedUpdate()
    {
        MoveEnemy(movement);  // Have enemy follow player
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
        }
        Debug.Log(facingLeft);
    }
}
