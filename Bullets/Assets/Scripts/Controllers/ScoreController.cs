using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    int totalScore = 0;
    //subscribing to entitykilled actions 
    private void OnEnable()
    {
        Actions.OnEnemyKilled += EnemyDead;
    }
    private void OnDisable()
    {
        Actions.OnEnemyKilled -= EnemyDead;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemyDead(Enemy enemyRef)
	{
        Debug.Log($"Enemy dead. Score: {totalScore += enemyRef.scoreValue}");
	}
}
