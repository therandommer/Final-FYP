using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplay : MonoBehaviour
{
    [SerializeField]
    public Player playerStats;
    public GameObject bulletSpawnPoint;
    int thisHealth;
    int thisShieldLevel;
    int thisWeaponLevel;
    int thisMaxWeaponLevel;
    bool updateWeapon;
    bool isFiring = false; //controls full auto
    [SerializeField]
    public GameObject equippedBullet;
    [SerializeField]
    public GameObject spawnPos;
    GameObject spawnParent;
    Vector2 mousePos = Vector2.zero;
    Vector2 mousePosWorld = Vector2.zero;
    Vector2 movement = Vector2.zero;
    float inputX = 0.0f;
    float inputY = 0.0f;
    bool isBoosting = false;
    bool isPaused = false;
    float angle = 0.0f; // angle used to go between player and mouse

    [SerializeField]
    Rigidbody2D rb = null;
    
    void OnEnable()
	{
        Actions.OnPause += TogglePause;
        Actions.OnCollectableAcquired += DropCollected;
    }
    void OnDisable()
    {
        Actions.OnPause -= TogglePause;
        Actions.OnCollectableAcquired -= DropCollected;
    }
    void Start()
    {
        thisHealth = playerStats.health;
        thisWeaponLevel = playerStats.weaponLevel;
        thisShieldLevel = playerStats.shieldLevel;
        thisMaxWeaponLevel = playerStats.bullets.Count;
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"No Rigid Body for player: " + this.name);
        }
        equippedBullet = playerStats.bullets[0];
        spawnParent = GameObject.Find("SpawnParent");
        if(!spawnParent)
		{
            Debug.LogError("No object of name 'SpawnParent' found in scene, add one to solve this error");
		}
        Actions.OnPlayerHit?.Invoke(thisHealth); //sets starting health value for ui
    }
    protected virtual void SpawnThis(Vector2 spawnPos)
    {
        Instantiate(this, spawnPos, Quaternion.identity, GameController.spawnHolder.transform);
    }
    protected virtual void SpawnBullet()
    {
        GameObject cloneBullet = Instantiate(equippedBullet, bulletSpawnPoint.transform.position, this.transform.rotation);
        cloneBullet.transform.parent = spawnParent.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(updateWeapon)
		{
            equippedBullet = playerStats.bullets[thisWeaponLevel-1];
            if(isFiring)
			{
                CancelInvoke("SpawnBullet");
                InvokeRepeating("SpawnBullet", 0, playerStats.startFireRate / thisWeaponLevel);
			}
            updateWeapon = false;
		}
        if(!isPaused)
		{
            mousePos = Input.mousePosition;
            mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");
            if (!isBoosting)
            {
                movement = new Vector2(inputX, inputY) * playerStats.moveSpeed;
            }
            else
            {
                movement = new Vector2(inputX, inputY) * playerStats.moveSpeed * playerStats.boostMult;
            }
            LookAtMouse(this.transform.position, mousePosWorld);
            if (Input.GetButtonDown("Fire1") && !isFiring)
            {
                isFiring = true;
                InvokeRepeating("SpawnBullet", 0, playerStats.startFireRate/thisWeaponLevel);
            }
            if (Input.GetButtonUp("Fire1") && isFiring)
            {
                isFiring = false;
                CancelInvoke("SpawnBullet");
            }
            if (Input.GetButtonDown("Boost"))
            {
                isBoosting = true;
            }
            if (Input.GetButtonUp("Boost"))
            {
                isBoosting = false;
            }
        }
    }

    void FixedUpdate()
    {
        if(!isPaused)
		{
            rb.velocity = movement;
        }
        if(isPaused && rb.velocity != Vector2.zero)
		{
            rb.velocity = Vector2.zero;
		}
    }

    void LookAtMouse(Vector3 a, Vector3 b) // a = player, b = cursor
	{
        float AngleRad = Mathf.Atan2(b.y - a.y, b.x - a.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
	}

    void DropCollected(Drop _thisDrop)
    {
        switch (_thisDrop.type)
        {
            case DropType.eHealth:
                thisHealth += _thisDrop.dropStrength;
                if (thisHealth > playerStats.health)
                    thisHealth = playerStats.health;
                Actions.OnPlayerHit?.Invoke(thisHealth); //reuse this action, just to update UI
                break;
            case DropType.eShield:
                thisShieldLevel += _thisDrop.dropStrength;
                if (thisShieldLevel > playerStats.maxShieldLevel)
                    thisShieldLevel = playerStats.maxShieldLevel;
                Actions.OnShieldGot?.Invoke(thisShieldLevel);
                break;
            case DropType.eWeapon:
                if (thisWeaponLevel > thisMaxWeaponLevel)
				{
                    thisWeaponLevel = thisMaxWeaponLevel; //maybe increase fire rate later?
                }
                else if (thisWeaponLevel < thisMaxWeaponLevel)
				{
                    thisWeaponLevel += _thisDrop.dropStrength;
                    if (thisWeaponLevel > thisMaxWeaponLevel)
                        thisWeaponLevel = thisMaxWeaponLevel;
                    updateWeapon = true;
                    
                }
                Actions.OnWeaponGot?.Invoke(thisWeaponLevel);
                break;
        }
    }
    public Vector2 GetForward()
	{
        return this.transform.forward;
	}

    void Die(Player thisPlayer)
	{
        Actions.OnPlayerKilled?.Invoke(playerStats); //triggered if not null
    }

    void TogglePause()
	{
        isPaused = !isPaused;
	}

    void Damage(int _Damage)
	{
        int newDamage = _Damage -= (thisShieldLevel - 1);
        if (newDamage < 0)
            newDamage = 1;
        thisHealth -= newDamage;
        Actions.OnPlayerHit?.Invoke(thisHealth);
    }
}
