using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This module satisfies:
 *   - Functional requirement 2.4
 */
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        base.ExitTheBoundary(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        base.HitABoundary(collision);
    }
}
