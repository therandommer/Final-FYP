using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameplay : AIGameplay
{
    public Bullet thisBullet;
    public GameObject childObj;
    BulletGameplay[] children;
    float timeLeft;
    Vector2 targetDestination = Vector2.zero;
    Vector2 targetDirection = Vector2.zero;
    bool acquiredTarget = false;
    bool isLookingTowards = false;
    float curveTimer = 0f;
    bool hasCollided = false;
    SpriteRenderer thisRenderer;
    void Awake()
    {
        thisRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(!thisRenderer)
		{
            Debug.LogError($"No sprite renderer for bullet: {this.name}. Adding fake sprite renderer");
            gameObject.AddComponent<SpriteRenderer>();
            thisRenderer = gameObject.GetComponent<SpriteRenderer>();
		}
    }
    void Start()
	{
        if(childObj != null)
		{
            children = GetComponentsInChildren<BulletGameplay>();
            foreach (BulletGameplay _children in children)
            {
                _children.SetProjectileSpeedScalar(projectileSpeedScalar);
            }
        }
    }
    void Update()
	{
        if(!hasInitialised)
		{
            Initialise(thisBullet);
            if(thisBullet.lifetime != 0)
			{
                timeLeft = thisBullet.lifetime;
                Invoke("Die", timeLeft);
            }
        }
        if(!acquiredTarget)
		{
            if(thisBullet.thisFaction == Bullet.BulletFaction.enemy)
			{
                GameObject player = GameObject.Find("Player");
                targetDestination = player.transform.position;
                targetDirection = (targetDestination - new Vector2(transform.position.x, transform.position.y)).normalized;
                if (targetDestination != Vector2.zero)
                {
                    acquiredTarget = true;
                }
            }
            else
			{
                acquiredTarget = true;
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
            if(!rb)
			{
                rb = gameObject.GetComponent<Rigidbody2D>();
			}
            
            switch (thisBullet.thisType)
			{
                case Bullet.BulletType.basic:
                    rb.velocity = transform.up * speed;
                    break;
                case Bullet.BulletType.towardsTarget:
                    if(!isLookingTowards)
					{
                        LookAtTarget(transform.position, targetDestination);
                        isLookingTowards = true;
                    }
                    transform.position += new Vector3(targetDirection.x, targetDirection.y) * thisBullet.moveSpeed * Time.deltaTime;
                    // transform.position = Vector2.MoveTowards(transform.position, targetDestination, thisBullet.moveSpeed * Time.deltaTime);
                    rb.velocity = transform.up * speed;
                    
                    break;
                case Bullet.BulletType.curveTowardsTarget:
                    transform.position = Bezier(curveTimer / 2, transform.position, new Vector2(transform.position.x, targetDestination.y), targetDestination);
                    rb.velocity = transform.up * speed;
                    LookAtTarget(transform.position, targetDestination);
                    curveTimer += Time.deltaTime;
                    break;
                default:
                    Debug.LogError($"Bullet {this.name} is not assigned a valid type");
                    break;
			}
            rb.velocity= rb.velocity * projectileSpeedScalar; //finally updates the projectile speed as needed in accordance to the song
        }
        if(isPaused && rb.velocity != Vector2.zero)
		{
            rb.velocity = Vector2.zero;
		}
    }
    void OnTriggerEnter2D(Collider2D col)
	{
        if(!hasCollided)
		{
            if (col.gameObject.tag == "Enemy" && thisBullet.thisFaction == Bullet.BulletFaction.player)
            {
                col.gameObject.SendMessage("Damage", thisBullet.damage);
                Actions.OnBulletHit?.Invoke(thisBullet);
                Hide();
            }
            if (col.gameObject.tag == "Player" && thisBullet.thisFaction == Bullet.BulletFaction.enemy)
            {
                //Debug.Log($"Dealing {thisBullet.damage} damage to player");
                col.gameObject.SendMessage("Damage", thisBullet.damage);
                Hide();
            }
        }
    }
    void Hide()
	{
        hasCollided = true;
        thisRenderer.enabled = !thisRenderer.enabled;
        if(childObj)
		{
            childObj.SetActive(false);
		}
	}
    void Die()
	{
        Destroy(gameObject);
	}
}
