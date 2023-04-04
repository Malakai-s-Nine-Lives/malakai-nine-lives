using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrollingEnemy : PatrollingEnemy
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
        allowedToTakeNextStep = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.HitABoundary(collision);
        allowedToTakeNextStep = false;
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        base.ExitTheBoundary(collision);
        allowedToTakeNextStep = true;
    }
}
