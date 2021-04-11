using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Enemy", order = 1)]
public class Enemy : Entity
{
    public int scoreValue = 20;
    public List<Vector2> movePoints; //used for movement lerps
    public List<GameObject> bullets;
    
    public float fireRate; //delay between each shot in seconds
    
    public enum StartDirection
    {
        left,
        right,
        up,
        down
    }
    public StartDirection thisDirection;
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetStartDirection(Enemy.StartDirection _direction)
    {
        Debug.Log($"Setting Direction to {_direction}");
        thisDirection = _direction;
    }
}
