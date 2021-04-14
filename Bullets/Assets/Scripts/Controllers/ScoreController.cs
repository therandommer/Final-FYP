using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    int totalScore = 0;
    public TextMeshProUGUI scoreText;
    //subscribing to entitykilled actions 
    private void OnEnable()
    {
        Actions.OnEnemyKilled += EnemyDead;
        Actions.OnBulletHit += BulletHit;
    }
    private void OnDisable()
    {
        Actions.OnEnemyKilled -= EnemyDead;
        Actions.OnBulletHit -= BulletHit;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BulletHit(Bullet _bulletRef)
	{
        if(scoreText)
		{
            scoreText.text = $"Score: {totalScore += _bulletRef.scoreValue}";
		}
	}
    void EnemyDead(Enemy _enemyRef)
	{
        if(scoreText)
		{
            scoreText.text = $"Score: {totalScore += _enemyRef.scoreValue}";
        }
	}
}
