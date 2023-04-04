using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPatrollingEnemy : PatrollingEnemy
{
    protected virtual void Start()
    {
        base.Start();
        allowedToTakeNextStep = true;
    }

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
        base.ExitTheBoundary(collision);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        base.HitABoundary(collision);
    }
}
