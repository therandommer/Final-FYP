using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameplay : AIGameplay
{
    public Bullet thisBullet;
    float timeLeft;
    void Start()
    {
        Initialise();
        movement = thisBullet.moveSpeed;
        sr.sprite = thisBullet.thisSprite;
        timeLeft = thisBullet.lifetime;
    }
    // Update is called once per frame
    void Update()
	{
        if(timeLeft>0.0f)
        {
            timeLeft -= Time.deltaTime;
		}
        if(timeLeft<= 0.0f)
		{
            Die();
		}
    }
    void FixedUpdate()
    {
        if(thisBullet.thisType == Bullet.BulletType.basic)
		{
            rb.velocity = transform.up * movement;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
	{
        if(col.gameObject.tag == "Enemy")
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
