using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGameplay : AIGameplay
{
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
        
    }

    void Update()
    {
        if(!hasInitialised)
		{
            Initialise(thisEnemy);
            thisMoveSpeed = thisEnemy.moveSpeed;
            //Debug.Log("Default health for enemy is" + health);
        }
        timeAlive += Time.deltaTime;
        if (health <=0)
        {
            Debug.Log($"The enemy: {this.name} is dying with {health} remaining");
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
    protected void Shoot()
	{
        int rnd = Random.Range(0, thisEnemy.bullets.Count);
        CheckFireDirection(thisEnemy.bullets[rnd].GetComponent<BulletGameplay>().thisBullet.thisFireDirection, rnd);
        GameObject cloneBullet = Instantiate(thisEnemy.bullets[rnd], gameObject.transform.position, fireDirection, gameObject.transform);
        if (!spawnParent)
        {
            spawnParent = GameObject.Find("SpawnParent");
        }
        cloneBullet.transform.parent = spawnParent.transform;
        //Debug.Log($"Firing bullet {rnd}");
    }
    protected void ShootRotation()
    {
        int rnd = Random.Range(0, thisEnemy.bullets.Count);
        CheckFireDirection(thisEnemy.bullets[rnd].GetComponent<BulletGameplay>().thisBullet.thisFireDirection, rnd);
        GameObject cloneBullet = Instantiate(thisEnemy.bullets[rnd], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        if(!spawnParent)
		{
            spawnParent = GameObject.Find("SpawnParent");
		}
        cloneBullet.transform.parent = spawnParent.transform;
        //Debug.Log($"Firing bullet {rnd}");
    }
    protected void Die(Enemy enemyRef)
	{
        Debug.Log($"Enemy {this.name} died");
        Actions.OnEnemyKilled?.Invoke(thisEnemy); //triggered if not null
        Destroy(gameObject);
    }
    
    protected void CheckDirection()
	{
        //Debug.Log($"Checking direction for enemy, direction is {thisEnemy.thisDirection}");
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
    protected void CheckFireDirection(Bullet.FireDirection _bulletDir, int bulletRef)
	{
        //Debug.Log($"Checking direction for bullet, direction is {_bulletDir}");
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
