using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldEnemy : EnemyGameplay
{
    public float holdTimer = 5.0f; //timer before enemy leaves
    bool startHolding = false;
    public int patrolLoops = 2; // loops before patrol enemy leaves
    bool isLeaving = false;
    public HoldEnemyEntity thisEntity;
    bool isCountingDown = false;

    void Start()
    {
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        InvokeRepeating("ShootRotation", thisEnemy.fireRate, thisEnemy.fireRate);
    }
    void FixedUpdate()
    {
        if (isCountingDown && holdTimer > 0 && !isPaused)
        {
            holdTimer -= Time.deltaTime;
        }
        if (holdTimer <= 0 && !isLeaving)
        {
            Leaving();
        }
        if (!isPaused)
		{
            if (Vector2.Distance(thisEntity.movePoints[positionNo], thisTransform) < pointRadius)
            {
                if (!startHolding)
                {
                    positionNo++;
                }
                if (positionNo >= thisEntity.movePoints.Count && patrolLoops != 0 && !startHolding) // reset to first position
                {
                    positionNo = 0;
                    patrolLoops--;
                }
                if (positionNo == 0 && patrolLoops == 0 && !startHolding) //when at first point again start leave count down after all loops
                {
                    startHolding = true;
                    isCountingDown = true;
                }
            }
            if (patrolLoops > 0)
            {
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), thisEntity.movePoints[positionNo], thisMoveSpeed * Time.deltaTime);
                LookAtTarget(transform.position, thisEntity.movePoints[positionNo]);
            }
            if (isLeaving)
            {
                Debug.Log("Leaving");
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, transform.position.y - 10), thisMoveSpeed * Time.deltaTime);
                LookAtTarget(transform.position, new Vector3(transform.position.x, transform.position.y - 10, 0));
            }
        }
    }
    void Leaving()
	{
        isLeaving = true;
	}
}
