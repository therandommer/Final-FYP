using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameplay : AIGameplay
{
    public Bullet thisBullet;
    float timeLeft;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
	{
        if(!hasInitialised)
		{
            Initialise(thisBullet);
            timeLeft = thisBullet.lifetime;
        }
        if(!isPaused)
		{
            if (timeLeft > 0.0f)
            {
                timeLeft -= Time.deltaTime;
            }
            if (timeLeft <= 0.0f)
            {
                Die();
            }
        }
        
        if (isPaused && rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
    }
    void FixedUpdate()
    {
        if(!isPaused)
		{
            if (thisBullet.thisType == Bullet.BulletType.basic)
            {
                rb.velocity = transform.up * speed;
            }
        }
        if(isPaused && rb.velocity != Vector2.zero)
		{
            rb.velocity = Vector2.zero;
		}
    }

    void OnTriggerEnter2D(Collider2D col)
	{
        if(col.gameObject.tag == "Enemy" && thisBullet.thisFaction == Bullet.BulletFaction.player)
		{
            Debug.Log($"Dealing {thisBullet.damage} damage to enemy");
            col.gameObject.SendMessage("Damage", thisBullet.damage);
            Die();
		}
        if (col.gameObject.tag == "Player" && thisBullet.thisFaction == Bullet.BulletFaction.enemy)
        {
            col.gameObject.SendMessage("Damage", thisBullet.damage);
            Die();
        }
    }
    void Die()
	{
        Destroy(gameObject);
	}
}
