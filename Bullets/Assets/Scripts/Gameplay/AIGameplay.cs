using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIGameplay : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Vector2 movement = Vector2.zero;
    protected SpriteRenderer sr;
    protected int health;
    void Start()
    {
        
    }
    protected void Initialise()
	{
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
