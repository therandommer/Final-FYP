using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Enemy", order = 1)]
public class Enemy : Entity
{
    public int scoreValue = 20;
    
    public List<GameObject> bullets;

    public List<Drop> availableDrops;
    public float dropChance;
    public float fireRate; //delay between each shot in seconds
    public enum StartDirection
    {
        left,
        right,
        up,
        down
    }
    public StartDirection thisDirection;
    public void SetStartDirection(BasicEnemyEntity.StartDirection _direction)
    {
        Debug.Log($"Setting Direction to {_direction}");
        thisDirection = _direction;
    }
    void Start()
    {
		
    }
    void Update()
    {
        
    }
    
}
