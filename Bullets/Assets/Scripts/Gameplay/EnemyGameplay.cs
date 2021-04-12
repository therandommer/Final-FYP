using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGameplay : AIGameplay
{
    [SerializeField]
    protected float thisMoveSpeed;
    public Enemy thisEnemy;
    protected int positionNo = 0;
    protected float pointRadius = 0.2f;
    protected Vector2 thisTransform;
    protected Vector2 defaultDirection;
    protected Quaternion fireDirection;
    protected float timeAlive = 0.0f;
    void Start()
    {
        Initialise();
        sr.sprite = thisEnemy.thisSprite;
        health = thisEnemy.health;
        thisMoveSpeed = thisEnemy.moveSpeed;
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

        }
        if (isPaused && rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
    }
    void Shoot()
	{
        int rnd = Random.Range(0, thisEnemy.bullets.Count);
        CheckFireDirection(thisEnemy.bullets[rnd].GetComponent<BulletGameplay>().thisBullet.thisFireDirection, rnd);
        Instantiate(thisEnemy.bullets[rnd], gameObject.transform.position, fireDirection, gameObject.transform);
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
    void CheckDirection()
	{
        Debug.Log($"Checking direction for enemy, direction is {thisEnemy.thisDirection}");
        switch (thisEnemy.thisDirection)
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
    }
    void CheckFireDirection(Bullet.FireDirection _bulletDir, int bulletRef)
	{
        Debug.Log($"Checkind direction for bullet, direction is {_bulletDir}");
        switch(thisEnemy.bullets[bulletRef].GetComponent<BulletGameplay>().thisBullet.thisFireDirection)
		{
            case Bullet.FireDirection.down:
                fireDirection = Quaternion.Euler(0, 0 ,-180);
                break;
            case Bullet.FireDirection.downLeft:
                fireDirection = Quaternion.Euler(0, 0, -135);
                break;
            case Bullet.FireDirection.downRight:
                fireDirection = Quaternion.Euler(0, 0, 135);
                break;
            case Bullet.FireDirection.up:
                fireDirection = Quaternion.Euler(0, 0, 0);
                break;
            case Bullet.FireDirection.upLeft:
                fireDirection = Quaternion.Euler(0, 0, 45);
                break;
            case Bullet.FireDirection.upRight:
                fireDirection = Quaternion.Euler(0, 0, -45);
                break;
            case Bullet.FireDirection.left:
                fireDirection = Quaternion.Euler(0, 0, 90);
                break;
            case Bullet.FireDirection.right:
                fireDirection = Quaternion.Euler(0, 0, -90);
                break;
            default:
                Debug.LogError($"Not valid fire direction given for enemy: {this.name}");
                break;
        }
	}
}
