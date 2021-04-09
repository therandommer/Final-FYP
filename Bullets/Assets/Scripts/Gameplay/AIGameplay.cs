using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIGameplay : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float speed;
    protected SpriteRenderer sr;
    protected int health;
    protected bool isPaused = false;
    void OnEnable()
    {
        Actions.OnPause += TogglePause;
    }
    void OnDisable()
	{
        Actions.OnPause -= TogglePause;
	}
    protected void Initialise()
	{
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
