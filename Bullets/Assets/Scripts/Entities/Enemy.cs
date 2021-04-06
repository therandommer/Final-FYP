using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Enemy", order = 1)]
public class Enemy : Entity
{
    public int scoreValue = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Die(Enemy enemyRef)
    {
        Actions.OnEnemyKilled?.Invoke(this); //triggered if not null
    }
}
