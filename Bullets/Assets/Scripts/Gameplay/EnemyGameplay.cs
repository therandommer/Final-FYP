using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGameplay : AIGameplay
{
    [SerializeField]
    float thisMoveSpeed;
    public Enemy thisEnemy;
    int positionNo = 0;
    float pointRadius = 0.2f;
    Vector2 thisTransform;
    void Start()
    {
        Initialise();
        sr.sprite = thisEnemy.thisSprite;
        health = thisEnemy.health;
        thisMoveSpeed = thisEnemy.moveSpeed;
    }

    void Update()
    {
        if (health<0)
        {
            Die(thisEnemy);
		}
        thisTransform.x = transform.position.x;
        thisTransform.y = transform.position.y;
       
    }
    void FixedUpdate() //basic movement for patroling enemy type. Will be normalised later
	{
        if (!isPaused)
        {
            if (transform.position != new Vector3(thisEnemy.movePoints[positionNo].x, thisEnemy.movePoints[positionNo].y, 0) && positionNo == 0) //sets initial position
            {
                transform.position = new Vector3(thisEnemy.movePoints[positionNo].x, thisEnemy.movePoints[positionNo].y, 0);
                positionNo++;
            }
            if (Vector2.Distance(thisEnemy.movePoints[positionNo], thisTransform) < pointRadius)
            {
                positionNo++;
                if (positionNo >= thisEnemy.movePoints.Count) // reset to first position
                {
                    positionNo = 1;
                }
            }
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), thisEnemy.movePoints[positionNo], thisMoveSpeed * Time.deltaTime);
        }
        if (isPaused && rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
    }
    void Die(Enemy enemyRef)
	{
        Actions.OnEnemyKilled?.Invoke(thisEnemy); //triggered if not null
        Destroy(gameObject);
    }
    void LookAtTarget(Vector3 a, Vector3 b) // a = player, b = cursor
    {
        float AngleRad = Mathf.Atan2(b.y - a.y, b.x - a.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }
}
