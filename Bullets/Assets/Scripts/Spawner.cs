using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SpawnRequirements
{
    public float activateTimer;//what time it will spawn the object
    public GameObject spawnObject;//object reference
    public int spawnNumber;//number of objects to spawn
    public float spawnDelay;//delay between each entity spawning
    public bool usesThisPos = false;
    public Vector2 actualSpawnPoint;//if not using the default position of this spawn point
}
public class Spawner : MonoBehaviour
{
    public List<SpawnRequirements> spawnReq;
    int currentSpawn = 0; //index number
    TimeController timeC;
    bool isSpawning = false;
    void OnEnable()
	{

	}
    void OnDisable()
	{

	}
    void Start()
    {
        timeC = FindObjectOfType<TimeController>();
        StartCoroutine(CheckTimer());
        spawnReq = spawnReq.OrderBy(w=>w.activateTimer).ToList(); //sort list by activation timer
    }

    IEnumerator CheckTimer()
	{
        
        yield return new WaitUntil(() => !timeC.isPaused); //holds coroutine until unpaused
	}
    
    IEnumerator SpawnObject()
	{
        if(!timeC.isPaused)
		{
            for(int i = 0; i<spawnReq[currentSpawn].spawnNumber; ++i)
			{
                if (spawnReq[currentSpawn].usesThisPos)
                {
                    Debug.Log("Spawning Object at position");
                    Instantiate(spawnReq[currentSpawn].spawnObject, gameObject.transform, false);
                }
                else if (!spawnReq[currentSpawn].usesThisPos)
                {
                    Debug.Log("Spawning Object at specified location");
                    Vector2 thisPosition = spawnReq[currentSpawn].actualSpawnPoint;
                    Instantiate(spawnReq[currentSpawn].spawnObject, thisPosition, this.transform.rotation, gameObject.transform);
                }
               
                yield return new WaitForSeconds(spawnReq[currentSpawn].spawnDelay);
            }
                currentSpawn++;
                isSpawning = false;
                Debug.Log($"Stopping spawn object coroutine, previous spawn was {currentSpawn} current spawn now {currentSpawn}");
                StopCoroutine(SpawnObject());
        }
        /*if(timeC.isPaused)
		{
            yield return new WaitUntil(() => !timeC.isPaused); //holds coroutine until unpaused
        }*/
    }
    void Update()
    {
        if (!timeC.isPaused)
        {
            if(currentSpawn<spawnReq.Count)
			{
                Debug.Log($"Checking Timer, time required: {spawnReq[currentSpawn].activateTimer} fpr spawn {currentSpawn}");
                if (timeC.timePassed >= spawnReq[currentSpawn].activateTimer && !isSpawning)
                {
                    Debug.Log($"Starting Spawn Coroutine at time {timeC.timePassed}");
                    isSpawning = true;
                    StartCoroutine(SpawnObject());
                }
                timeC.timeToNextSpawn = spawnReq[currentSpawn].activateTimer - timeC.timePassed;
            }
        } 
        
        
    }
}
