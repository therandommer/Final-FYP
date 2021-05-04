using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIGameplay : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float speed;
    [SerializeField]
    protected float projectileSpeedScalar = 1.0f;
    protected SpriteRenderer sr;
    [SerializeField]
    protected int health;
    protected bool isPaused = false;
    protected bool hasInitialised = false;
    protected GameObject spawnParent;
    void OnEnable()
    {
        Actions.OnPause += TogglePause;
        Actions.OnLevelRestart += Reset;
        Actions.OnNewBPMSpeed += GetProjScalar;
    }
    void OnDisable()
	{
        Actions.OnPause -= TogglePause;
        Actions.OnLevelRestart -= Reset;
        Actions.OnNewBPMSpeed -= GetProjScalar;
    }
    protected void Initialise(Enemy _thisEnemy)
	{
        health = _thisEnemy.health;
        //Debug.Log("Enemy now has: " + health + " health");
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
    protected void LookAtTarget(Vector3 a, Vector3 b) // a = player, b = cursor
    {
        float AngleRad = Mathf.Atan2(b.y - a.y, b.x - a.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
    }
    protected Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c) //curve over a period of 0.0 -> 1.0
	{
        Vector2 ab = Vector2.Lerp(a, b, t);
        Vector2 bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
	}
    protected void Damage(int _damage)
	{
        health -= _damage;
        //Debug.Log($"Damaged{this.name} for {_damage} damage");
	}
    void TogglePause()
	{
        isPaused = !isPaused;
        //Debug.Log($"Toggling pause for {this.name}");
	}
    
    void Reset()
	{
        Destroy(gameObject);
	}
    void GetProjScalar(int _index) //gets and sets the projectile scalar, only called on the bpm update ticks
	{
        projectileSpeedScalar = FindObjectOfType<GameController>().GetExistingIntensity(_index);
    }
    protected void SetProjectileSpeedScalar(float _newScalar)
	{
        projectileSpeedScalar = _newScalar;
	}
    public float GetProjectileSpeedScalar()
	{
        return projectileSpeedScalar;
	}
}
