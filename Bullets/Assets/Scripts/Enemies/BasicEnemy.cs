using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this enemy moves in one of 8 directions until the end of its life cycle
public class BasicEnemy : EnemyGameplay
{
    void Start()
    {
        if(rb == null)
		{
            rb = gameObject.GetComponent<Rigidbody2D>();
		}
        Invoke("CheckDirection", 0.2f);
        InvokeRepeating("Shoot", 0.2f, thisEnemy.fireRate);
    }
    void FixedUpdate()
    {
        if(!isPaused)
		{
            rb.velocity = defaultDirection;
        }
    }
}
