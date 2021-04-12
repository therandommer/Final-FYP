using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PatrolEnemy will move between a set of points, instantly spawning them at pos1
public class PatrolEnemy : EnemyGameplay
{
    public int patrolLoops = 2; // loops before patrol enemy leaves
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        /*if (transform.position != new Vector3(thisEnemy.movePoints[positionNo].x, thisEnemy.movePoints[positionNo].y, 0) && positionNo == 0) //sets initial position
        {
            transform.position = new Vector3(thisEnemy.movePoints[positionNo].x, thisEnemy.movePoints[positionNo].y, 0);
            positionNo++;
        }*/
        if (Vector2.Distance(thisEnemy.movePoints[positionNo], thisTransform) < pointRadius)
        {
            positionNo++;
            if (positionNo >= thisEnemy.movePoints.Count && patrolLoops != 0) // reset to first position
            {
                positionNo = 0;
                patrolLoops--;
            }
            
        }
        if(patrolLoops >0)
		{
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), thisEnemy.movePoints[positionNo], thisMoveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Leaving");
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, transform.position.y - 10), thisMoveSpeed * Time.deltaTime);
        }
    }
}
