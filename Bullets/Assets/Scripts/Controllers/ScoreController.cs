using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    int totalScore = 0;
    public int scoreMultiplier = 1;
    public float playerHitPenalty = 0.66f;
    int defaultScoreMultiplier = 1; //will be altered by mods, etc. in the future, will increase the maximum multipliers, etc. 
    public int maximumScoreMultiplier = 20;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public Color defaultScoreColours = Color.white;
    public Color boostedScoreColours = Color.magenta;
    ModController thisModMultiplier;
    public float modMultiplier = 1.0f;
    //subscribing to entitykilled actions 
    private void OnEnable()
    {
        Actions.OnEnemyKilled += EnemyDead;
		Actions.OnBulletHit += BulletHit;
        Actions.OnCollectableAcquired += DropCollected;
        Actions.OnPlayerHit += ReduceMultiplier;
        Actions.OnLevelRestart += Reset;
    }
    private void OnDisable()
    {
        Actions.OnEnemyKilled -= EnemyDead;
        Actions.OnBulletHit -= BulletHit;
        Actions.OnCollectableAcquired -= DropCollected;
        Actions.OnPlayerHit -= ReduceMultiplier;
        Actions.OnLevelRestart -= Reset;
    }
    void Start()
	{
        scoreMultiplier = defaultScoreMultiplier;
        if (thisModMultiplier == null)
            thisModMultiplier = FindObjectOfType<ModController>();
	}
    void Update()
	{
        if(scoreMultiplier == maximumScoreMultiplier && multiplierText.color != boostedScoreColours)
		{
            multiplierText.color = boostedScoreColours;
            scoreText.color = boostedScoreColours;
		}
        else if(scoreMultiplier < maximumScoreMultiplier && multiplierText.color != defaultScoreColours)
		{
            multiplierText.color = defaultScoreColours;
            scoreText.color = defaultScoreColours;
        }
	}
    void DropCollected(Drop _thisDrop)
	{
        if(scoreText && multiplierText)
		{
            scoreText.text = $"Score: {totalScore += Mathf.RoundToInt(_thisDrop.scoreValue * (scoreMultiplier * thisModMultiplier.GetModScoreMultiplier()))}";
            multiplierText.text = $"X {scoreMultiplier += _thisDrop.multiplierIncrease}";
            if (scoreMultiplier > maximumScoreMultiplier)
			{
                scoreMultiplier = maximumScoreMultiplier;
                multiplierText.text = $"X {scoreMultiplier}";
            }
        }
	}
    void ReduceMultiplier(int _ref) //multiply score by penalty, then round down
	{
        float tmpScoreMulti = scoreMultiplier;
        tmpScoreMulti *= 0.66f;
        scoreMultiplier = Mathf.FloorToInt(tmpScoreMulti);
        if (scoreMultiplier < 1)
            scoreMultiplier = 1;
        multiplierText.text = $"X {scoreMultiplier}";
	}
    void BulletHit(Bullet _bulletRef)
	{
        if(scoreText && multiplierText)
		{
            scoreText.text = $"Score: {totalScore += Mathf.RoundToInt(_bulletRef.scoreValue * (scoreMultiplier * thisModMultiplier.GetModScoreMultiplier()))}";
            //Debug.Log("Current Multiplier is: " + scoreMultiplier * thisModMultiplier.GetModScoreMultiplier());
		}
	}
    void EnemyDead(Enemy _enemyRef)
	{
        if(scoreText && multiplierText)
		{
            scoreText.text = $"Score: {totalScore += Mathf.RoundToInt(_enemyRef.scoreValue * (scoreMultiplier * thisModMultiplier.GetModScoreMultiplier()))}";
        }
	}
    void Reset()
	{
        totalScore = 0;
        scoreMultiplier = defaultScoreMultiplier;
        scoreText.text = "Score: 0";
        multiplierText.text = $"X: {defaultScoreMultiplier}";
	}
}
