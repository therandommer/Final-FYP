using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameSpawnController : MonoBehaviour
{
    public TextMeshProUGUI thisSpeedText;
    [SerializeField]
    Spawner[] spawnPoints;
    public GameUIController thisSpeed;
    public ModController thisMod;
    public TimeController thisTime;
    //auto spawning system and additional mods (currently just difficulty decreasing/increasing time between spawns)
    float difficultyScalar = 1;
    public float defaultSpawnInterval = 5.0f; //spawns an enemy every interval, modified by intensity, if bpm is higher than 120 and mod, will decrease on time passed too
    public float minimumSpawnInterval = 2.0f;
    public float maximumSpawnInterval = 8.0f;
    float spawnInterval = 0.5f;
    public float offsetScalar = 1.0f;
    public float startSpawnTimer = 2.0f;
    float spawnTimer = 0.0f;
    int bonusEnemies = 0; // increase by 1 every minute
    int difficultyEnemies = 0; //higher on harder
    bool isStopped = false;
    void OnEnable()
	{
        Actions.OnNewBPMSpeed += ModifySpawnOffset;
        Actions.OnLevelComplete += ToggleStopped;
        Actions.OnLevelStart += EnableThis;
        Actions.OnPlayerKilled += ToggleStopped2;
	}
    void OnDisable()
	{
        Actions.OnNewBPMSpeed -= ModifySpawnOffset;
        Actions.OnLevelComplete -= ToggleStopped;
        Actions.OnLevelStart -= EnableThis;
        Actions.OnPlayerKilled -= ToggleStopped2;
    }
    void Start()
    {
        if (!thisMod)
            thisMod = FindObjectOfType<ModController>();
        if (!thisSpeed)
            thisSpeed = FindObjectOfType <GameUIController>();
        if (!thisTime)
            thisTime = FindObjectOfType<TimeController>();
        spawnPoints = FindObjectsOfType<Spawner>();
        switch(thisMod.GetDifficulty())
		{
            case Difficulty.eEasy:
                difficultyScalar = 0.5f;
                break;
            case Difficulty.eNormal:
                difficultyScalar = 1.0f;
                difficultyEnemies = 1;
                break;
            case Difficulty.eHard:
                difficultyScalar = 2.0f;
                difficultyEnemies = 1;
                break;
            default:
                break;
		}
        CalculateNewOffset();
        spawnTimer = startSpawnTimer;
    }
    void CalculateNewOffset()
	{
        spawnInterval = (defaultSpawnInterval / difficultyScalar) / (thisSpeed.GetSpeedNumber() * offsetScalar);
        if (spawnInterval < minimumSpawnInterval)
            spawnInterval = minimumSpawnInterval;
        if (spawnInterval > maximumSpawnInterval)
            spawnInterval = maximumSpawnInterval;
    }
    void Update()
    {
        if(!isStopped)
		{
            if (thisTime.timePassed > 0)
            {
                spawnTimer -= Time.deltaTime;
            }
            if (spawnTimer <= 0.0f)
            {
                SendSpawnMessage();
                spawnTimer = spawnInterval;
            }
            bonusEnemies = Mathf.FloorToInt(thisTime.timePassed / 60 + difficultyEnemies) + 1;
            thisSpeedText.text = "Spawn Interval " + spawnInterval + " Enemies = " + bonusEnemies;
        }
    }
    void ModifySpawnOffset(int _index)
	{
        CalculateNewOffset();
	}
    void SendSpawnMessage()
	{
        int tmpI = Random.Range(0, spawnPoints.Length);
        Debug.Log("Spawning "+ bonusEnemies +" Enemies");
        spawnPoints[tmpI].SpawnEnemy(bonusEnemies);
	}
    void ToggleStopped()
	{
        isStopped = !isStopped;
	}
    void ToggleStopped2(GameObject playerRef)
    {
        isStopped = !isStopped;
    }
    void EnableThis()
	{
        isStopped = false;
	}
}
