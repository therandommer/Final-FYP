using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIGameplay : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float speed;
    protected SpriteRenderer sr;
    [SerializeField]
    protected int health;
    protected bool isPaused = false;
    protected bool hasInitialised = false;
    protected GameObject spawnParent;
    void OnEnable()
    {
        Actions.OnPause += TogglePause;
    }
    void OnDisable()
	{
        Actions.OnPause -= TogglePause;
	}
    protected void Initialise(Enemy _thisEnemy)
	{
        health = _thisEnemy.health;
        Debug.Log("Enemy now has: " + health + " health");
        //sr.sprite = _thisEnemy.thisSprite;
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError($"AI: {gameObject.name} has no rigidbody assigned");
        }
        sr = GetComponent<SpriteRenderer>();
        if (!sr)
        {
            Debug.LogError($"No sprite renderer for object: {this.name}");
        }
        spawnParent = GameObject.Find("SpawnParent");
        if (!spawnParent)
        {
            Debug.LogError("No object of name 'SpawnParent' found in scene, add one to solve this error");
        }
        hasInitialised = true;
    }
    protected void Initialise(Bullet _thisBullet)
    {
        speed = _thisBullet.moveSpeed;
        //sr.sprite = _thisBullet.thisSprite;
        
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError($"AI: {gameObject.name} has no rigidbody assigned");
        }
        sr = GetComponent<SpriteRenderer>();
        if (!sr)
        {
            Debug.LogError($"No sprite renderer for object: {this.name}");
        }
        spawnParent = GameObject.Find("SpawnParent");
        if (!spawnParent)
        {
            Debug.LogError("No object of name 'SpawnParent' found in scene, add one to solve this error");
        }
    }

    protected void Damage(int _damage)
	{
        health -= _damage;
        Debug.Log($"Damaged{this.name} for {_damage} damage");
	}
    
    void TogglePause()
	{
        isPaused = !isPaused;
        Debug.Log($"Toggling pause for {this.name}");
	}
}
