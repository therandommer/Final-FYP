using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    int totalScore = 0;
    public int scoreDropMultiplier = 2;
    int defaultScoreDropMultiplier = 2;
    public TextMeshProUGUI scoreText;
    //subscribing to entitykilled actions 
    private void OnEnable()
    {
        Actions.OnEnemyKilled += EnemyDead;
		Actions.OnBulletHit += BulletHit;
        Actions.OnCollectableAcquired += DropCollected;
        Actions.OnLevelRestart += Reset;
    }
    private void OnDisable()
    {
        Actions.OnEnemyKilled -= EnemyDead;
        Actions.OnBulletHit -= BulletHit;
        Actions.OnCollectableAcquired -= DropCollected;
        Actions.OnLevelRestart -= Reset;
    }
    void Start()
	{
        defaultScoreDropMultiplier = scoreDropMultiplier;
	}
    void DropCollected(Drop _thisDrop)
	{
        if(scoreText)
		{
            if(_thisDrop.type != DropType.eScore)
			{
                scoreText.text = $"Score: {totalScore += _thisDrop.scoreValue}";
            }
            else
                scoreText.text = $"Score: {totalScore += _thisDrop.scoreValue * scoreDropMultiplier}";
        }
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
    void Reset()
	{
        totalScore = 0;
        scoreDropMultiplier = defaultScoreDropMultiplier;
        scoreText.text = "Score: 0";
	}
}
