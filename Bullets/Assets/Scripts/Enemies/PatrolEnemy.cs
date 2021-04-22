using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PatrolEnemy will move between a set of points
public class PatrolEnemy : EnemyGameplay
{
    public int patrolLoops = 2; // loops before patrol enemy leaves
    public PatrolEnemyEntity thisEntity;
    void Start()
    {
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        InvokeRepeating("ShootRotation", thisEntity.fireRate, thisEntity.fireRate);
    }

    void FixedUpdate()
    {
        if(!isPaused)
		{
            if (Vector2.Distance(thisEntity.movePoints[positionNo], thisTransform) < pointRadius)
            {
                positionNo++;
                if (positionNo >= thisEntity.movePoints.Count && patrolLoops != 0) // reset to first position
                {
                    positionNo = 0;
                    patrolLoops--;
                }

            }
            if (patrolLoops > 0)
            {
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), thisEntity.movePoints[positionNo], thisMoveSpeed * Time.deltaTime);
                LookAtTarget(transform.position, thisEntity.movePoints[positionNo]);
            }
            else
            {
                //Debug.Log("Leaving");
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, transform.position.y - 10), thisMoveSpeed * Time.deltaTime);
                LookAtTarget(transform.position, new Vector3(transform.position.x, transform.position.y - 10, 0));
            }
        }
    }
}
