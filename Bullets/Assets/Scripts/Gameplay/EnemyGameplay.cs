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
    Vector2 defaultDirection;
    float timeAlive = 0.0f;
    void Start()
    {
        Initialise();
        sr.sprite = thisEnemy.thisSprite;
        health = thisEnemy.health;
        thisMoveSpeed = thisEnemy.moveSpeed;
        switch(thisEnemy.thisDirection)
		{
            case Enemy.StartDirection.down:
                defaultDirection = Vector2.down * thisMoveSpeed;
                break;
            case Enemy.StartDirection.up:
                defaultDirection = Vector2.up * thisMoveSpeed;
                break;
            case Enemy.StartDirection.left:
                defaultDirection = Vector2.left * thisMoveSpeed;
                break;
            case Enemy.StartDirection.right:
                defaultDirection = Vector2.right * thisMoveSpeed;
                break;
            default:
                Debug.LogError($"No direction set for enemy: {this.name}");
                break;
		}
        InvokeRepeating("Shoot", 0.2f, thisEnemy.fireRate);
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
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
            rb.velocity = defaultDirection;
            //use this for a patrolling enemy type with fixed movement. Maybe a boss?
            /*if (transform.position != new Vector3(thisEnemy.movePoints[positionNo].x, thisEnemy.movePoints[positionNo].y, 0) && positionNo == 0) //sets initial position
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
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), thisEnemy.movePoints[positionNo], thisMoveSpeed * Time.deltaTime);*/
        }
        if (isPaused && rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
    }
    void Shoot()
	{
        int rnd = Random.Range(0, thisEnemy.bullets.Count);
        Instantiate(thisEnemy.bullets[rnd], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        Debug.Log($"Firing bullet {rnd}");
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
