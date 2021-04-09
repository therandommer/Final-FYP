using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGameplay : AIGameplay
{
    public Enemy thisEnemy;
    
    void Start()
    {
        Initialise();
        sr.sprite = thisEnemy.thisSprite;
        health = thisEnemy.health;
    }

    // Update is called once per frame
    void Update()
    {
        if(health<0)
        {
            Die(thisEnemy);
		}
    }

    void Die(Enemy enemyRef)
	{
        Actions.OnEnemyKilled?.Invoke(thisEnemy); //triggered if not null
        Destroy(gameObject);
    }
}
